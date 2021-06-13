using System.Collections.Generic;
using Quze.Models.Models.Alerts;

namespace Quze.SendAlerts
{
   public abstract class SendMessage
   {
        /// <summary>
        ///  collects all the alerts to send
        /// </summary>

        protected SendMessage()
        {
        }


        /// <summary>
        /// contains the alerts to send
        /// </summary>
        public List<IAlert> AlertsToSend { get; set; }

        public abstract void SendAll();

        /// <summary>
        ///  sends a single alert
        /// </summary>
        /// <param name="alertToSend"></param>
        protected abstract void Send(IAlert alertToSend);

    }
}
