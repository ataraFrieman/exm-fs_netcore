using System;
using System.Collections.Generic;
using Quze.Models.Entities;
using Quze.Models.Models.Alerts;

namespace SendAlerts.Tests
{
   public static class DemoAlertsList
    {
        public static List<IAlert> GetTestAlrtesList()
        {
            var list = new List<IAlert>();
            var alert1 = new Alert
            {
                ActualSentTime = null,
                AlertType = AlertType.SMS,
                MessageBody = "עליך להתחיל לצום בעוד 2 שעות",
                MessageTitle = "לקראת הניתוח",
                TimeToSend = DateTime.Now.AddHours(-2),
                To = "+972586300016"
            };

            var alert2 = new Alert
            {
                ActualSentTime = null,
                AlertType = AlertType.Email,
                MessageBody = "עליך להתחיל לצום בעוד 8 שעות",
                MessageTitle = "לקראת הניתוח",
                TimeToSend = DateTime.Now.AddHours(-3),
                To = "jacov141@gmail.com"
            };
            var alert4 = new Alert
            {
                ActualSentTime = null,
                AlertType = AlertType.SMS,
                MessageBody = "עליך להתחיל לצום בעוד 2 שעות",
                MessageTitle = "לקראת הניתוח",
                TimeToSend = DateTime.Now.AddHours(-5),
                To = "+972549446170"
            };
            var alert5 = new Alert
            {
                ActualSentTime = null,
                AlertType = AlertType.SMS,
                MessageBody = "עליך להתחיל לצום בעוד 2 שעות",
                MessageTitle = "לקראת הניתוח",
                TimeToSend = DateTime.Now.AddHours(-1),
                To = "0583902992"
            };
            var alert6 = new Alert
            {
                ActualSentTime = null,
                AlertType = AlertType.Email,
                MessageBody = "עליך להתחיל לצום בעוד 2 שעות",
                MessageTitle = "לקראת הניתוח",
                TimeToSend = DateTime.Now.AddHours(-3),
                To = "efraim@quze.ai"
            };

            list.Add(alert1);
            list.Add(alert2);
            list.Add(alert6);
            list.Add(alert4);
            list.Add(alert5);

            return list;
        }
    }
}
