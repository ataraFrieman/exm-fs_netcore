using System;
using Quze.DAL;
using Quze.Infrastruture.Utilities;
using Quze.Infrastruture.Utilities.Email;
using Quze.SendAlerts;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;
using Serilog;
using System.Threading.Tasks;

namespace Quze.SendAlertsService
{
    class AlertsSendService
    {
        QuzeContext ctx;
        private DAL.DALForDelete dal;

        private SendEmail emailSender;
        private SendSMS smsSender;

        public AlertsSendService()
        {
            Init();
        }

        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.Console()
                 .WriteTo.File("logs\\SendAlertsService.txt", rollingInterval: RollingInterval.Day)
                 .CreateLogger();
            Log.Information("------------------ Main starts -------------------------");
            try
            {
                var alertsSendService = new AlertsSendService();
                await alertsSendService.GetAndSend();
            }
            catch (Exception ex)
            {
                Log.Error(ex, " ==================================>");
            }
            Log.Information("Main end");

        }


        private void Init()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            DbContextOptionsBuilder contextOptionsBuilder = new DbContextOptionsBuilder();
            var options = contextOptionsBuilder.UseSqlServer(connectionString).Options;
            ctx = new QuzeContext(options, new BatchUserService(User.SendAlertsServiceUser));
            dal = new DAL.DALForDelete(ctx);
        }

        private async Task<bool> GetAndSend()
        {
            Log.Information("Starting Sending Alerts");
            var list = await dal.GetUnsentAlertsAsync();
            Log.Information("Alert to send: {0}", list.Count);
            emailSender = new SendEmail(new GmailMail())
            {

                AlertsToSend = list
            };
            smsSender = new SendSMS
            {
                SmsHandler = new SlngSMS(),
                AlertsToSend = list
            };
            Log.Information("sending SMS");
            smsSender.SendAll();
            Log.Information("sending emails");
            emailSender.SendAll();
            return true;
        }

    }
}
