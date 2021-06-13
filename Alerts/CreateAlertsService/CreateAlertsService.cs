using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quze.CreateAlerts;
using Quze.DAL;
using Quze.Models.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.CreateAlertsService
{
    class CreateAlertsService
    {
        QuzeContext ctx;
        private DALForDelete dal;
        private CreateAppointmentsAlert createAppointmentAlerts;
        private List<Appointment> appointments;


        public CreateAlertsService()
        {
            Init();
        }

        private void Init()
        {
            var directory = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
               .SetBasePath(directory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            DbContextOptionsBuilder contextOptionsBuilder = new DbContextOptionsBuilder();
            var options = contextOptionsBuilder.UseSqlServer(connectionString).Options;
            ctx = new QuzeContext(options, new BatchUserService(User.CreateAlertsServiceUser));
            dal = new DAL.DALForDelete(ctx);
        }

        private async Task<bool> CreateTheAlertsAsync()
        {
            Log.Information("Starting CreateTheAlertsAsync");
            appointments = await dal.GetUpcomingAppointmentsAsync(DateTime.Now);
            Log.Information("Appointments count: {0}", appointments.Count);
            createAppointmentAlerts = new CreateAppointmentsAlert(appointments, DateTime.Now);
            var alerts = createAppointmentAlerts.GetAllAppointmentsAlerts();
            Log.Information("Total alerts count: {0}", alerts.Count());
            dal.SaveAlertsAsync(alerts);
            Log.Information("CreateTheAlertsAsync end");
            return true;
        }

        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.Console()
                 .WriteTo.File("logs\\CreateAlertsService.txt", rollingInterval: RollingInterval.Day)
                 .CreateLogger();

            Log.Information("------------------ Main starts -------------------------");
            try
            {
                var createAlertsService = new CreateAlertsService();
                await createAlertsService.CreateTheAlertsAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, " ==================================>");
            }

            Log.Information("Main end");
        }
    }
}
