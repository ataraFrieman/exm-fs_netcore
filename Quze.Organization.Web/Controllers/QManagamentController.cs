using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Quze.Models.Entities;
using Quze.Models.Logic;
using Quze.Organization.Web.ViewModels;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.Organization.Web.Helpers;
using Microsoft.Extensions.Logging;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class QManagamentController : BaseController
    {
        public class AdvanceQRequest
        {
            public int ServiceQueueId { get; set; }
        }
        private static string websitUrl = "";
        private readonly IMemoryCache cache;
        protected readonly ILogger _logger;

        public QManagamentController(QuzeContext ctx, IMemoryCache memoryCache, IMapper mapper, IConfiguration configuration, ILogger<QManagamentController> logger) : base(ctx, mapper)
        {
            _logger = logger;
            cache = memoryCache;


            websitUrl = configuration.GetConnectionString("WebsiteUrl");
        }

        private void UpdateAppointments(ServiceQueue serviceQ, int actualDurationDifference)
        {

            ServiceQueue currentQ = context.ServiceQueues.Include(s => s.Appointments)
                .FirstOrDefaultAsync(q => q.Id == serviceQ.Id).Result;
            if (currentQ == null)
                return;

            List<AppointmentPushVM> appointmentPushVMs = new List<AppointmentPushVM>();
            var serviceQueueLogic = new ServiceQueueLogic(currentQ);


            serviceQueueLogic.CalculatePositionInQueue();
            serviceQueueLogic.UpdateDelayToAllAppointmentsInQ(actualDurationDifference);
            serviceQueueLogic.CalculateEtts();
            serviceQueueLogic.CalculateNextPush();
            currentQ = serviceQueueLogic.ServiceQueue;
            context.SaveChangesAsync();

            //nextAppointments = serviceQueueLogic.RelevantAppointments;
            if (serviceQueueLogic.RelevantAppointments != null && serviceQueueLogic.RelevantAppointments.Count > 0)
            {
                for (int i = 0; i < serviceQueueLogic.RelevantAppointments.Count; i++)
                {
                    AppointmentPushVM appointmentPushVM = new AppointmentPushVM();
                    appointmentPushVM.PositionInQueue = serviceQueueLogic.RelevantAppointments[i].PositionInQueue;
                    appointmentPushVM.Id = serviceQueueLogic.RelevantAppointments[i].Id;
                    appointmentPushVM.FellowId = serviceQueueLogic.RelevantAppointments[i].FellowId;
                    appointmentPushVM.Fellow = mapper.Map<FellowVM>(serviceQueueLogic.RelevantAppointments[i].Fellow);
                    appointmentPushVM.NextPush = serviceQueueLogic.RelevantAppointments[i].NextPush;
                    appointmentPushVM.CurrentTime = DateTime.Now;
                    appointmentPushVM.Delay = serviceQueueLogic.RelevantAppointments[i].Delay;
                    appointmentPushVM.ETTS = serviceQueueLogic.RelevantAppointments[i].ETTS;
                    appointmentPushVM.ActualBeginTime = serviceQueueLogic.RelevantAppointments[i].ActualBeginTime;
                    appointmentPushVMs.Add(appointmentPushVM);
                }
                WebsiteRequest.SendRequestToWebsiteServer(appointmentPushVMs, websitUrl + "qManagament/UpdateAppointmentsBySignalR", _logger);

            }


        }



        [HttpGet]
        public IEnumerable<ServiceQueueVM> GetQueues(int ServiceProviderId = -1, int idStart = 0)
        {
            var OrganizationId = JwtUser.OrganizationId;
            List<ServiceQueue> queuesList = null;
            try
            {
                queuesList = context.ServiceQueues
               .Include(q => q.ServiceType)
               .Include(q => q.ServiceStation)
               .Include(q => q.CurrentAppointement)
               .Include(q => q.Branch)
               .Include(q => q.Appointments)
               .ThenInclude(q => q.Fellow)
                .Where(q => (ServiceProviderId > 0 ? q.ServiceProviderId == ServiceProviderId :
                (q.OrganizationId == OrganizationId)) && (idStart != 0 ? q.Id > idStart : q.Id != 0) && q.BeginTime.Month == DateTime.Now.Month&& q.BeginTime.Year == DateTime.Now.Year)
                .OrderByDescending(q => q.BeginTime)
                .Take(10)
                .ToList();
                for (int i = 0; i < queuesList.Count; i++)
                {
                    if(queuesList[i].ServiceProviderId>1)
                    queuesList[i].ServiceProvider = context.ServiceProviders
                        //.Include(q => q.ServiceProvidersServiceTypes)
                        //.ThenInclude(q => q.ServiceType)
                        .Include(q => q.Organization)
                        .FirstOrDefault(s => s.id == queuesList[i].ServiceProviderId);
                        
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            if (queuesList != null)
                foreach (var queue in queuesList)
                {
                    if (queue.ServiceProvider != null && queue.ServiceProvider.ServiceProvidersServiceTypes != null && queue.ServiceProvider.ServiceProvidersServiceTypes.Count > 0)
                    {
                        var ST = queue.ServiceProvider.ServiceProvidersServiceTypes.FirstOrDefault(s => s.ServiceType.ParentServiceId >= 1);
                        if (ST != null)
                            queue.ServiceType = ST.ServiceType;
                    }
                    foreach (var appointment in queue.Appointments)
                    {
                        appointment.ServiceQueue = null;
                    }
                    queue.Appointments = queue.Appointments.OrderBy(m => m.BeginTime).ToList();
                }
            List<ServiceQueueVM> queuesVM = mapper.Map<List<ServiceQueueVM>>(queuesList);
            return queuesVM;
        }

        [HttpPost("[action]")]
        public IActionResult StartQueue([FromBody]AdvanceQRequest request)
        {
            var serviceQ = context.ServiceQueues.Include(sq => sq.ServiceProvider)
            .Include(sq => sq.Appointments).ThenInclude(a => a.Fellow)
            .Include(sq => sq.ServiceType)
            .Include(sq => sq.ServiceStation)
            .Include(sq => sq.CurrentAppointement)
                .FirstOrDefault(q => q.Id == request.ServiceQueueId);
            if (serviceQ != null)
            {
                ServiceQueueLogic serviceQueueLogic = new ServiceQueueLogic(serviceQ);
                serviceQ = serviceQueueLogic.StartQueue();
                context.SaveChanges();
            }
            ServiceQueueVM queueVM = mapper.Map<ServiceQueueVM>(serviceQ);
            return Ok(queueVM);
        }

        protected async System.Threading.Tasks.Task<int> UpdateSQDBAsync(ServiceQueue serviceQ)
        {
            Appointment currentAppointment = serviceQ.Appointments.LastOrDefault(a => a.ActualBeginTime != null);
            Appointment NextAppointment = serviceQ.Appointments.FirstOrDefault(a => a.ActualBeginTime == null);
            bool isFirstInQueue = currentAppointment == null;
            if (currentAppointment != null)
            {
                currentAppointment.ActualEndTime = DateTime.Now;

                DateTime actualBegin = currentAppointment.ActualBeginTime.GetValueOrDefault();
                currentAppointment.ActualDuration = Convert.ToInt32((DateTime.Now - actualBegin).TotalSeconds);
            }
            else
                serviceQ.ActualBeginTime = DateTime.Now;
            //any case update next appointment
            if (NextAppointment != null)
            {
                serviceQ.CurrentAppointement = NextAppointment;
                serviceQ.CurrentAppointementId = NextAppointment.Id;
                NextAppointment.ActualBeginTime = DateTime.Now;
            }
            //last appointment
            else
            {
                serviceQ.Passed = true;
                serviceQ.ActualEndTime = DateTime.Now;
            }
            await context.SaveChangesAsync();
            int TimeDifference = 0;
            if (isFirstInQueue)
                TimeDifference = Convert.ToInt32(((DateTime.Now - serviceQ.BeginTime).TotalSeconds));
            else
                if (NextAppointment != null)
                TimeDifference = Convert.ToInt32((currentAppointment.ActualDuration - currentAppointment.Duration));
            return TimeDifference;
        }

        [HttpPost("[action]")]
        public IActionResult AdvanceQ([FromBody]AdvanceQRequest request)
        {
            var serviceQ = context.ServiceQueues.Include(sq => sq.ServiceProvider)
            .Include(sq => sq.Appointments).ThenInclude(a => a.Fellow)
            .Include(sq => sq.ServiceType)
            .Include(sq => sq.ServiceStation)
            .Include(sq => sq.CurrentAppointement)
            .FirstOrDefault(q => q.Id == request.ServiceQueueId);
            if (serviceQ.Passed)
                return Ok("Q passed");
            int TimeDifference = 0;
            if (serviceQ != null && serviceQ.Appointments != null && serviceQ.Appointments.Count > 0)
            {
                serviceQ.Appointments = serviceQ.Appointments.OrderBy(x => x.BeginTime.TimeOfDay).ToList();
                TimeDifference = UpdateSQDBAsync(serviceQ).Result;
                if (Math.Abs(TimeDifference) >= 60)
                    UpdateAppointments(serviceQ, TimeDifference);
                if (serviceQ.CurrentAppointement != null)
                    serviceQ.CurrentAppointement.ServiceQueue = null;
                foreach (var item in serviceQ.Appointments)
                {
                    item.ServiceQueue = null;
                }
                ServiceQueueVM queueVM = mapper.Map<ServiceQueueVM>(serviceQ);
                return Ok(queueVM);
            }
            else
                return Ok("No service queue or no appointments found(" + request.ServiceQueueId);
        }


    }
}
