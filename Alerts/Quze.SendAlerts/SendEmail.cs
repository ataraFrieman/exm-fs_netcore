using System;
using System.Linq;
using System.Threading.Tasks;
using Quze.Infrastruture.Utilities.Email;
using Quze.Models.Models.Alerts;

namespace Quze.SendAlerts
{
    
    public class SendEmail : SendMessage
    {
       
        private readonly IEmail emailSender;
        /// <summary>
        ///  default ctor
        /// </summary>
        public SendEmail( IEmail emailSender)
        {
           
            this.emailSender = emailSender;
        }


        /// <summary>
        ///     Sends all the list after checking the alert type
        /// </summary>
        public override void SendAll()
        {
            var emailAlerts = AlertsToSend.Where(a => a.AlertType == AlertType.Email).ToArray();
            Parallel.ForEach(emailAlerts, Send);
        }


        /// <inheritdoc />
        /// <summary>
        /// Sends a single sms alert
        /// </summary>
        /// <param name="alertToSend"></param>
        protected override void Send(IAlert alertToSend)
        {
            alertToSend.Validate();
            emailSender.SendMessageAsync(alertToSend.To, alertToSend.MessageTitle, alertToSend.MessageBody);
            alertToSend.ActualSentTime = DateTime.Now;
        }

    }
}

