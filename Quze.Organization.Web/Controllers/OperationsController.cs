using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Quze.BL.Operations;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models;
using Quze.Models.Models;
using Quze.Models.Entities;
using Quze.Organization.Web.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quze.Models.Logic;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class OperationsController : BaseController
    {
        private readonly IMemoryCache cache;
        private readonly OperationQueueStore operationsStore;
        private readonly ServiceProviderStore serviceProviderStore;
        private readonly ServiceQueueStore serviceQueueStore;
        private EditOperationLogic editOperationLogic { get; set; }
        
        private OpeartionsLogic opeartionsLogic;
        private readonly IConfiguration configuration;

        private readonly string sqlDMConnection;
        const int Duration = 30;

        private Quze.Models.Response<ServiceStationVM> ServiceStationResponse;
        private Quze.Models.Response<DepartmentsVM> DepartmentsResponse;
        private Quze.Models.Response<OperationVM> OperationResponse;
        private Quze.Models.Response<CancelationReasonsVM> CancelationReasonsResponse;

        enum AppointmentsType : int
        {
            Anesthesia = 500,
            Cleaning = 501
        };

        int AnesthesiaDuration = 1000;
        int CleanDuration = 500;
        int SurgeyDuration = 3000;


        public OperationsController(QuzeContext ctx, IMapper mapper, IMemoryCache _cache, IConfiguration configuration) : base(ctx, mapper)
        {
            this.cache = _cache;
            opeartionsLogic = new OpeartionsLogic(context, configuration);
            this.configuration = configuration;
            sqlDMConnection = configuration.GetConnectionString("DMConnection");
            editOperationLogic = new EditOperationLogic(ctx);
            operationsStore = new OperationQueueStore(ctx);
            serviceProviderStore = new ServiceProviderStore(ctx);
            serviceQueueStore = new ServiceQueueStore(ctx);
        }

        public class ActualOperationRecord
        {
            public string OperationCode { get; set; }
            public DateTime AnesthesiaActualBeginTime { get; set; }
            public DateTime AnesthesiaActualEndTime { get; set; }
            public DateTime CleanActualEndTime { get; set; }
            public DateTime CleanActualBeginTime { get; set; }
            public DateTime SurgeryActualBeginTime { get; set; }
            public DateTime SurgeryActualEndTime { get; set; }
        }

        public class AddOperationRequest : Request<OperationRecord>
        {
            public DateTime BeginTime;
            public ServiceQueue ServiceQueue;
            public int? OrganizationId;
        }

        public class UploadActualRequest
        {
            public List<ActualOperationRecord> ActaulOPList { get; set; }
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }




        [HttpPost]
        public async Task<OperationsResponseVM> Post([FromBody] AddOperationsRequest request)
        {
            //בדיקה שהנתונים שהגיעו תקינים אם לא להחזיר שגיאה
            if (request.Entities == null || request.Entities.Count == 0)
                return null;
            opeartionsLogic = new OpeartionsLogic(context, configuration);
            request.OrganizationId = JwtUser.OrganizationId;
            opeartionsLogic.request = request;
            OperationsResponse operationsResponse = await opeartionsLogic.CraeteOperationsQueue(sqlDMConnection, request.State);
            var rS = mapper.Map<OperationsResponseVM>(operationsResponse);
            return rS;
        }



        //emergency surgery
        [HttpPost("[action]")]
        public async Task<OperationsResponseVM> AddOperation([FromBody] AddOperationsRequest request)
        {
            //TODO: OperationsResponseVM operationsResponseVM = new OperationsResponseVM();
            if (request.Entity == null)
                return null;
            opeartionsLogic = new OpeartionsLogic(context, configuration);
            opeartionsLogic.request = request;
            request.OrganizationId = JwtUser.OrganizationId;
            request.BeginTime = request.Entity.BeginTime;
            var response = await opeartionsLogic.AddOperation(request.ServiceQueueId, request.BeginTime, request.OrganizationId.Value, request.Entity, request.State);
            response.ServiceQueue.Appointments = null;
            var rS = mapper.Map<OperationsResponseVM>(response);
            return rS;
        }

        [HttpPost("[action]")]
        public TeamReady UpdateTeamReady([FromBody] IsTeamReady IsTeamReady)
        {

            try
            {
                
               return opeartionsLogic.UpdateTeamReady(IsTeamReady);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpPost("[action]")]
        public List<Operation> UploadActual([FromBody] UploadActualRequest request)
        {
            List<Operation> Operationslist = new List<Operation>();
            for (int i = 0; i < request.ActaulOPList.Count; i++)
            {
                var Item = request.ActaulOPList[i];
                var operation = context.Operations.FirstOrDefault(o => o.Code == Item.OperationCode);
                operation.CleanActualBeginTime = Item.CleanActualBeginTime;
                operation.CleanActualEndTime = Item.CleanActualEndTime;
                operation.AnesthesiaActualBeginTime = Item.AnesthesiaActualBeginTime;
                operation.AnesthesiaActualEndTime = Item.AnesthesiaActualEndTime;
                operation.SurgeryActualBeginTime = Item.SurgeryActualBeginTime;
                operation.SurgeryActualEndTime = Item.SurgeryActualEndTime;
                Operationslist.Add(operation);

            }
            context.SaveChangesAsync();
            return Operationslist;
        }

        [HttpPost("[action]")]
        public bool UpdadateStatusApointment([FromBody] Appointment apointment)
        {
            try
            {
                Console.Write(apointment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }





        public int GetAnesthesiaDuration(int surgeryDuration)
        {
            if (surgeryDuration <= 60 * 60)
                return 3 * 60;
            else if (surgeryDuration <= 120 * 60)
                return 5 * 60;
            else
                return 5 * 60;

        }

        public int GetCleaningDuration(int surgeryDuration)
        {
            //if (surgeryDuration <= 60 * 60)
            //    return 5 * 60;
            //else if (surgeryDuration <= 120 * 60)
            //    return 10 * 60;
            //else
            //    return 10 * 60;
            return 15 * 60;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }


        [HttpGet("[action]")]
        public Quze.Models.Response<ServiceStationVM> GetRooms()
        {
            ServiceStationResponse = new Quze.Models.Response<ServiceStationVM>();

            var organizationId = JwtUser.OrganizationId;
            List<ServiceStation> ServiceStation = operationsStore.GetRooms(organizationId ?? 0);
            List<ServiceStationVM> ServiceStationVM = mapper.Map<List<ServiceStationVM>>(ServiceStation);

            ServiceStationResponse.Entities = ServiceStationVM;
            return ServiceStationResponse;
        }

        [HttpGet("[action]")]
        public Quze.Models.Response<DepartmentsVM> GetDepartments()
        {
            DepartmentsResponse = new Quze.Models.Response<DepartmentsVM>();

            var organizationId = JwtUser.OrganizationId;
            List<Departments> Departments = operationsStore.GetDepartments(organizationId.Value);
            List<DepartmentsVM> DepartmentsVM = mapper.Map<List<DepartmentsVM>>(Departments);
            DepartmentsResponse.Entities = DepartmentsVM;
            return DepartmentsResponse;
        }

        [HttpPost("[action]")]
        public OperationsResponseVM DeleteOperation([FromBody] EditRequest editRequest)
        {
            try
            {
                OperationsResponse operationsResponse = editOperationLogic.DeleteOperation(editRequest.OperationQueue, editRequest.State, editRequest.OldAppointment);
                var rS = mapper.Map<OperationsResponseVM>(operationsResponse);
                return rS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


       

        [HttpGet("[action]/{id}")]
        public OperationsResponseVM GetServiceQueueById(int id)
        {
            var organizationId = JwtUser.OrganizationId;
            var serviceQueue= opeartionsLogic.GetServiceQueuById(id, organizationId);
            var sQ= mapper.Map<OperationsResponseVM>(serviceQueue);
            return sQ;
        }

        [HttpGet("[action]")]
        public IEnumerable<ServiceQueueVM> GetQueuesDates()
        {
            var organizationId = JwtUser.OrganizationId;
            List<ServiceQueue> queues;
            try
            {
                queues = context.ServiceQueues
                .Where(q => q.OrganizationId == organizationId)
                .OrderByDescending(q => q.BeginTime)
                //Take(10)
                .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<ServiceQueueVM> queuesVM = mapper.Map<List<ServiceQueueVM>>(queues);
            return queuesVM;
        }

        [HttpGet("[action]")]
        public Quze.Models.Response<CancelationReasonsVM> GetCancelationReasons()
        {
            CancelationReasonsResponse = new Quze.Models.Response<CancelationReasonsVM>();
            var organizationId = JwtUser.OrganizationId;
            List<CancelationReasons> CancelationReasons = operationsStore.GetReasons(organizationId ?? 0);
            List<CancelationReasonsVM> CancelationReasonsVM = mapper.Map<List<CancelationReasonsVM>>(CancelationReasons);
            CancelationReasonsResponse.Entities = CancelationReasonsVM;
            return CancelationReasonsResponse;
        }

        [HttpPost("[action]")]
        public Response<Operation> CancelOperationById([FromBody] CancelRequest request)
        {
            var response = new Response<Operation>();
            try
            {
                if (request.OperationId == 0)
                {
                    response.AddError(Infrastruture.Utilities.ErrorCodes.NullRequest, "OperationId is empty!");
                    return response;
                }
                if (request.ReasonId == 0)
                {
                    response.AddError(Infrastruture.Utilities.ErrorCodes.NullRequest, "ReasonId is empty!");
                    return response;
                }
                var operation = operationsStore.CanceleOperation(request.OperationId, request.ReasonId);
                //var appointment = context.Appointments.Where(app => app.OperationId == operation.Id);
                response.Entity = operation;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return response;
        }

        [HttpPost("[action]")]
        public Response<ServiceQueue> CreateNewShift([FromBody] NewShoft request)
        {
            request.OrganizationId = JwtUser.OrganizationId;

            var response = new Response<ServiceQueue>();
            try
            {
                if (request.Date == null)
                {
                    response.AddError(Infrastruture.Utilities.ErrorCodes.NullRequest, "Date is empty!");
                    return response;
                }
                if (request.StartTime == null)
                {
                    response.AddError(Infrastruture.Utilities.ErrorCodes.NullRequest, "StartTime is empty!");
                    return response;
                }

                var time = request.Date.ToString("d") + " " + request.StartTime.ToString("HH:mm");
                var beginTime = DateTime.Parse(time);
                time = request.Date.ToString("d") + " " + request.EndTime.ToString("HH:mm");
                var endTime = DateTime.Parse(time);
                var serviceQueue = serviceQueueStore.CreateOpeartionQueue(beginTime, request.OrganizationId.Value, request.EndTime);
                //ServiceQueueVM serviceQueueVM = mapper.Map<ServiceQueue>(serviceQueue);
                response.Entity = serviceQueue;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;


        }
        public class CancelRequest
        {
            public int OperationId { get; set; }
            public int ReasonId { get; set; }
        }
        public class NewShoft
        {
            public DateTime Date { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int? OrganizationId;
        }
    }
}
