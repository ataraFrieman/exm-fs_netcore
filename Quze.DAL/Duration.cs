using Quze.Models.ML;
using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Quze.Infrastruture.Utilities;

namespace ML
{
    public class Duration
    {
        public static int requestId = 10000;
        //private static readonly HttpClient client = new HttpClient();

        public async Task<ResponseML> GetDurationAsync(RequestML request)
        {
            try
            {
                request.RequestId = requestId++;
                //TODO: the VisitNumber should not by hardcoded
                request.VisitNumber = 1;
                request.FellowBirthDate = request.FellowBirthDateAndTime.ToString("yyyy-MM-dd");
                var url = AppConfiguration.AppSettings("ML_URL");
                var json = JsonConvert.SerializeObject(request);




                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ResponseML>(response);
                }

            }
            catch (Exception e)
            {
                //TODO: return real answer Not random
                var response = new ResponseML() { responseDuration = (new Random(DateTime.Now.GetHashCode()).Next(5, 30)) * 60, RequestId = request.RequestId, IsError = 1 };
                response.AddServiceError(e.Message);
                return response;
            }
        }


    }



}
