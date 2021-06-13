using System;
using Quze.Models.Models.Alerts;
using Quze.Infrastruture.Utilities;
using System.Threading.Tasks;
using System.Linq;

namespace Quze.SendAlerts
{
    /// <summary>
    ///     Service to send list of alerts via SMS
    /// </summary>
    public class SendSMS :SendMessage
    {
        public ISMS SmsHandler { get; set; }


        /// <summary>
        ///  default ctor
        /// </summary>
        public SendSMS()
        {
        }




        /// <summary>
        /// Sends all the list after checking the alert type
        /// </summary>
        public override void SendAll()
        {
            var smsAlerts = AlertsToSend.Where(a => a.AlertType == AlertType.SMS).ToArray();
            Parallel.ForEach(smsAlerts, Send);
        }



        /// <inheritdoc />
        /// <summary>
        ///  Sends a single sms alert
        /// </summary>
        /// <param name="alertToSend"></param>
        protected override void Send(IAlert alertToSend)
        {
            SmsHandler.SendMessage(alertToSend.To, alertToSend.MessageTitle, alertToSend.MessageBody, null);
            alertToSend.ActualSentTime = DateTime.Now;
        }

    }
}
