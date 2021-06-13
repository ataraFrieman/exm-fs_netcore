using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Quze.Organization.Web.ViewModels;
using Quze.Models.Logic;
using System;
using Quze.Models;
using Quze.DAL;
using Quze.Infrastruture.Security;
using System.Threading.Tasks;
using Quze.Infrastruture.Extensions;
using Microsoft.AspNetCore.Identity;
using Quze.Models.Entities;
using System.Threading;
using Quze.DAL.Stores;
using Quze.Infrastruture.Utilities;
using Microsoft.Extensions.Configuration;
using Quze.Organization.Web.Helpers;
using Microsoft.Extensions.Logging;

namespace Quze.Organization.Web.Controllers
{
    public class AppointmentsController : BaseController
    {

        private readonly IMapper mapper;
        private readonly QueueStore queueStore;
        private readonly OrganizationStore organizationStore;
        private readonly DAL.Stores.UserStore userStore;
        public ISMS smsHandler { get; set; }
        private readonly IConfiguration configuration;
        private readonly ILogger _logger;

        public AppointmentsController(QuzeContext ctx,
            IMapper mapper,
            QueueStore queueStore,
            OrganizationStore organizationStore,
            IUserService userService,
            IUserStore<User> userStore,
            IConfiguration configuration,
            ILogger<AppointmentsController> logger
            ) : base(ctx, mapper)
        {
            this.queueStore = queueStore;
            this.organizationStore = organizationStore;
            this.mapper = mapper;
            this.userStore = (UserStore)userStore;
            this.configuration = configuration;
            smsHandler = new SlngSMS();
            _logger = logger;
        }

        public class AppointmentRequest
        {
            public int UserId { get; set; }
        }


        [HttpPost("[action]")]
        public async Task<IEnumerable<AppointmentVM>> GetUserAppointments([FromBody]AppointmentRequest request)
        {
            var now = Time.GetNow();
            try
            {
                var user = await userStore.FindByIdAsync(JwtUser.Id.ToString(), UserType.OrganizationUser, CancellationToken.None);
                var fellowIds = user.Fellows.Select(f => f.Id).ToList();
                var serviceQueueLogic = await queueStore.GetUserAppointmentsServiceQ(user);
                var appointments = serviceQueueLogic.SelectMany(sq => sq.RelevantAppointments).Where(a => fellowIds.Contains(a.FellowId)).ToList();

                List<AppointmentVM> appointmentsVM = mapper.Map<List<AppointmentVM>>(appointments);
                appointmentsVM.ForEach(a =>
                {
                    var serviceQueue = serviceQueueLogic.FirstOrDefault(sq => sq.Id == a.ServiceQueueId);
                });

                return appointmentsVM;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        [HttpPost("[action]")]
        public int GetAppointmentsNumToUser([FromBody]AppointmentRequest request)
        {
            var appointments = context.Appointments.Where(q => q.Fellow.IdentityNumber == context.Users.FirstOrDefault(u => u.Id == request.UserId).IdentityNumber && q.Served == false && q.ActualBeginTime == null).ToList();
            return appointments != null && appointments.Count > 0 ? appointments.Count : 0;
        }


        [HttpPost("[action]")]
        public async Task<Response<AppointmentVM>> SetAppointment([FromBody] Request<SetAppointmentVM> request)
        {
            _logger.LogInformation("SetAppointment: {Message}", "start");
            var response = new Response<AppointmentVM>();
            var AppointmentRequest = request.Entity;

            var organizationId = JwtUser.OrganizationId;
            var organization = await organizationStore.GetByIdAsync(organizationId.Value);
            Fellow fellow = await context.Fellows
                .Where(f => f.Id.ToString() == AppointmentRequest.FellowIdentityNumber && f.OrganizationId == organizationId)
                .FirstOrDefaultAsync();


            if (fellow.IsNull() || fellow.Id == 0)
            {
                fellow.Id=CreateFellow(AppointmentRequest.FellowIdentityNumber).Id;
            }
            Appointment appointment = null;
            try
            {
                appointment = await queueStore.SaveAppointmentAsync(
                   fellow.Id,
                   AppointmentRequest.ServiceTypeId,
                   AppointmentRequest.ServiceProviderId.Value,
                   AppointmentRequest.BranchId,
                   AppointmentRequest.BeginTime,
                   fellow.BirthDate ?? new DateTime(2000, 1, 1),
                   0,
                   AppointmentRequest.ServiceQueueId
                   ); 
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SetAppointment Error:( : {Message}", ex.Message);
                Console.WriteLine(ex.Message);
                return response;
            }
            _logger.LogInformation("SetAppointment saved appointmet id:( : {Message}", appointment.Id.ToString());

            appointment.Fellow = fellow;
            appointment.ETTS = appointment.BeginTime;
            response.Entity = mapper.Map<AppointmentVM>(appointment);
            appointment.ServiceQueue.Appointments = null;
            var WebsiteClientUrl = configuration.GetConnectionString("WebsiteClientUrl");
            WebsiteClientUrl += "appointments/" + appointment.Id;
            System.Threading.Tasks.Task.Run(() => smsHandler.SendAppointmentDetails(AppointmentRequest.PhoneNumber,
               organization.Name,
               response.Entity.ServiceProvider.FullName,
               response.Entity.BeginTime,
               appointment.ServiceQueue.Branch.Name,
               WebsiteClientUrl));
            //var appointmentToPush = serviceQueueLogic.SelectMany(sq => sq.RelevantAppointments).Where(a => fellowIds.Contains(a.FellowId)).ToList();
            _logger.LogInformation("SetAppointment send to update : {Message}", configuration.GetConnectionString("WebsiteUrl") + "qManagament/PushNewAppointmentsBySignalR");

            WebsiteRequest.SendRequestToWebsiteServer(appointment, (configuration.GetConnectionString("WebsiteUrl") + "qManagament/PushNewAppointmentsBySignalR"), _logger);

            return response;
        }


        Fellow CreateFellow(string identityNumber)
        {
            Fellow fellow = new Fellow()
            { OrganizationId = JwtUser.OrganizationId.Value, IdentityNumber = identityNumber, Email = JwtUser.UserName };
            FellowStore FS = new FellowStore(context);
            Fellow fellowStore=FS.AddFellow(fellow);
            return fellowStore;
        }
    }
}
