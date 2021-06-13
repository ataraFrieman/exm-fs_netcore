using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Models.Logic;
using Quze.Models.ML;

namespace Quze.API.Controllers
{

    [Route("[controller]")]
    public class MLController : ControllerBase
    {
        //string URL = "http://localhost:5000/api/mlrequest";
        string URL = "";
        ServiceProvidersServiceTypeStore store;
        IConfiguration configuration;
        public MLController(ServiceProvidersServiceTypeStore store, IConfiguration configuration)
        {
            this.store = store;
            this.configuration = configuration;

            URL = configuration.GetSection("AppSettings").GetValue<string>("ML_URL");
        }
        [Route("work")]
        public string GetDur()
        {
            return "hi its work";
        }

        [Route("queue-management")]
        public List<ResponseML> QueueManagement([FromBody]List<RequestML> request)
        {
            var response = new List<ResponseML>();
            request.ForEach(async req =>
            {
                var rsp = await AppointmentRequestAsync(req);
                rsp.RequestId = req.RequestId;
                response.Add(rsp);
            });
            return response;
        }
        [Route("get-duration")]
        public async Task<ResponseML> GetDurationAsync([FromBody] OperationRequestML request)
        {
            string jsn = JsonConvert.SerializeObject(request);
            using (var client = new HttpClient())
            {
                try
                {

                    var rsp = await client.PostAsync(URL, new StringContent(jsn, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ResponseML>(rsp);
                    response.responseDuration = Round5Minutes(response.responseDuration.Value);
                    response.responseDelayDuration = response.responseDelayDuration.Value;
                    if (response.responseDuration == 0)
                        response.responseDuration = 110;

                    response.SetAsDelayRecommendation();
                    response.AsDelay = (int)response.responseHasDelay;
                    response.responseHasDelay = response.HasDelayProb;
                    return response;
                }
                catch (Exception EX)
                {

                    throw;
                }
                
            }
            
        }

        [Route("schedule")]
        public async Task<ResponseML> AppointmentRequestAsync([FromBody]RequestML request)//**
        {
            if (request.AppointmentTime.IsNull())
                return ErrorResponse();

            if (request.ScheduledDate.IsNull())
            {
                request.ScheduledDate = DateTime.Now;
            }

            //TODO add reccomendation when visitNumber = 0

            string jsn = JsonConvert.SerializeObject(request);

            FillRequestFromQFlowDB(request);
            try
            {
                return await RequestToML(jsn, request);
            }

            catch (Exception e)
            {
                return await ErrorMLResonse(request, e);
            }
        }

        private void FillRequestFromQFlowDB(RequestML request)
        {
            var ds = QNomyClient.DAL.GetCustomer(request.FellowId.ToString());
            var table = ds.Tables[0];
            if (table.IsNotNull())
            {
                var row = table.Rows[0];
                if (row.IsNotNull())
                {
                    String dateOfBirthStr = row["DOB"].IsNotNull() ? row["DOB"].ToString() : "01/01/1970 00:00:00";
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    DateTime dateOfBirthDate = DateTime.ParseExact(dateOfBirthStr, "dd/MM/yyyy HH:mm:ss", culture);
                    request.FellowBirthDate = dateOfBirthDate.ToString("yyyy-MM-dd");
                    string gender = row["Sex"].IsNotNull() ? row["Sex"].ToString() : "1";
                    request.FellowGender = gender == "2" ? "F" : "M";
                }
            }
        }


        private async Task<ResponseML> ErrorMLResonse(RequestML request, Exception e)
        {
            int avgDuration = await GetAvgDuration(request);
            var duration = Round5Minutes(avgDuration / 60);
            var response = new ResponseML() { AsDelay = -1, IsError = 1, responseDuration = duration };
            response.AddServiceError(e.Message);
            return response;
        }

        private async Task<ResponseML> RequestToML(string jsn, RequestML request)
        {
            using (var client = new HttpClient())
            {
                var rsp = await client.PostAsync(URL, new StringContent(jsn, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ResponseML>(rsp);
                response.responseDuration = Round5Minutes(response.responseDuration.Value);
                response.SetAsDelayRecommendation();
                response.AsDelay = (int)response.responseHasDelay;
                response.responseHasDelay = response.HasDelayProb;
                return response;
            }
        }

        private async Task<int> GetAvgDuration(RequestML request)
        {
            try
            {
                return await store.GetAvgDurationAsync(request.ServiceProviderId.Value, request.ServiceTypeId.Value);
            }

            catch (Exception)
            {
                return 0;
            }

        }

        private int Round5Minutes(int duration)
        {
            if (duration % 5 == 0)
            {
                return duration;
            }

            return duration + (5 - duration % 5);
        }

        ResponseML ErrorResponse()
        {
            var response = new ResponseML() { IsError = 1 };
            response.AddServiceError("appointment time can't be null");
            return response;
        }

        public void GetDurationML(List<Appointment> operatinList)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Parallel.ForEach(operatinList, (operatin) =>
                    {
                        //נצרך פניה לפונקצית חישוב דיןריישן של  שמעון
                        //operatin= await client.PostAsync(URL, new StringContent(jsn, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                        //שמירה בדטהה בייס ועדכון שגיאה  
                    });
                }
            }
            catch (Exception ex)
            {
                //עדכון שגיאה 
            }
            //אם חזר בלי שגיאה אז עדכון סיום קובץ בהצלחה
        }
        [Route("get-str")]
        public string GetStr() { return "hello"; }
    }

}