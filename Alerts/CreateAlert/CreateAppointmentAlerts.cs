using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Models.Models.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quze.CreateAlerts
{
    public class CreateAppointmentAlerts
    {
        Appointment appointment;
        DateTime now;
        TimeSpan hoursBeforeAppointment;

        //TODO: Asher, please make sure to pass user or a deviceId
        public User User { get; set; }

        public CreateAppointmentAlerts(Appointment appointment, DateTime? now = null)
        {
            this.appointment = appointment;
            this.now = now.IsNull() ? DateTime.Now : now.Value;
            hoursBeforeAppointment = appointment.BeginTime - this.now;
        }

        public List<Alert> GetAppointmentAlerts()
        {
            //TODO: rethink alerts after appointment beginTime
            if (appointment.BeginTime < now) // wrong time
            {
                return null;
            }

            var result = new List<Alert>();


            var alertRrules =
                appointment.AppointmentTasks
                .Where(at => at.TimeApproved == null)
                .SelectMany(at => at.RequiredTask.AlertRules)
                .ToList();

            alertRrules.AddRange(
                appointment.AppointmentDocs.Where(at => at.TimeApproved == null)
                .SelectMany(a => a.RequiredDocument.AlertRules)
             );

            var types = Enum.GetValues(typeof(AlertType));
            foreach (AlertType type in types)
            {

                var alertsOfType = alertRrules.Where(r => r.AlertTypeID == type).ToList();
                var alert = new Alert();
                switch(type)
                {
                    case AlertType.SMS:
                        alert = GetSMSCombinedAlert(alertsOfType);
                        break;
                    case AlertType.Email:
                        alert = GetEmailCombinedAlert(alertsOfType);
                        break;
                    case AlertType.APP:
                        alert = GetAppCombinedAlert(alertsOfType);
                        break;
                    case AlertType.IVR:
                        alert = GetIVRCombinedAlert(alertsOfType);
                        break;
                    default:
                        break;
                }

                if (alert.IsNotNull())
                {
                    alert.AlertType = type;
                    result.Add(alert);
                }
            }

            return result;
        }

        private Alert GetSMSCombinedAlert(List<AlertRule> alertRulesOfType)
        {

            var alerts = new List<Alert>();

            foreach (var rule in alertRulesOfType)
            {
                var alert = GetAlertFromRule(rule);
                if (alert.IsNotNull())
                {
                    alerts.Add(alert);
                }
            }

            if (alerts.IsNullOrEmpty()) return null;
            if (alerts.Count == 1) return alerts.FirstOrDefault();

            var result = CombineAlerts(alerts);
            result.AlertType = AlertType.SMS;
            result.To = appointment.Fellow.PhoneNumber;
            return result;
        }

        private Alert GetEmailCombinedAlert(List<AlertRule> alertRulesOfType)
        {

            var alerts = new List<Alert>();

            foreach (var rule in alertRulesOfType)
            {
                var alert = GetAlertFromRule(rule);
                if (alert.IsNotNull())
                {
                    alerts.Add(alert);
                }
            }

            if (alerts.IsNullOrEmpty()) return null;
            if (alerts.Count == 1) return alerts.FirstOrDefault();

            var result = CombineAlerts(alerts);
            result.AlertType = AlertType.Email;
            result.To = appointment.Fellow?.Email;
            return result;
        }

        private Alert GetAppCombinedAlert(List<AlertRule> alertRulesOfType)
        {

            var alerts = new List<Alert>();

            foreach (var rule in alertRulesOfType)
            {
                var alert = GetAlertFromRule(rule);
                if (alert.IsNotNull())
                {
                    alerts.Add(alert);
                }
            }

            if (alerts.IsNullOrEmpty()) return null;
            if (alerts.Count == 1) return alerts.FirstOrDefault();

            var result = CombineAlerts(alerts);
            result.AlertType = AlertType.APP;
            //result.To = appointment.Fellow.ApplicationId;
            return result;
        }

        private Alert GetIVRCombinedAlert(List<AlertRule> alertRulesOfType)
        {

            var alerts = new List<Alert>();

            foreach (var rule in alertRulesOfType)
            {
                var alert = GetAlertFromRule(rule);
                if (alert.IsNotNull())
                {
                    alerts.Add(alert);
                }
            }

            if (alerts.IsNullOrEmpty()) return null;
            if (alerts.Count == 1) return alerts.FirstOrDefault();

            var result = CombineAlerts(alerts);
            result.AlertType = AlertType.IVR;
            result.To = appointment.Fellow.PhoneNumber;
            return result;
        }

        private Alert CombineAlerts(List<Alert> alerts)
        {
            string message = string.Empty;
            List<int> alertRulesIDs = new List<int>();
            var combinedAlert = alerts.FirstOrDefault();
            for (int i = 1; i < alerts.Count; i++)
            {
                combinedAlert.MessageBody += Environment.NewLine + alerts[i].MessageBody;
                combinedAlert.AlertRulesId += "," + alerts[i].AlertRulesId;
            }
            return alerts[0];
        }

        private Alert GetAlertFromRule(AlertRule rule)
        {
            if (rule.HoursBeforeToAlert < hoursBeforeAppointment.TotalHours)
            {
                return null;
            }

            var existingAlerts = new List<Alert>();
                //appointment.Alerts?.                    FindAll(x => x.AlertRulesId.FirstOrDefault(y => y == rule.Id) != 0);

            if (existingAlerts != null)
            {
                var thereAreUnsentAlerts = existingAlerts.Any(a => a.ActualSentTime == null);
                if (thereAreUnsentAlerts)
                {
                    return null;
                }

                var lastAlert = existingAlerts.OrderBy(x => x.ActualSentTime).FirstOrDefault();

                var timeFromLastAlert = (lastAlert.ActualSentTime - now).Value.Hours;

                if (timeFromLastAlert < rule.RepeatInterval)
                {
                    return null;
                }
            }

            var alert = new Alert()
            {
                MessageBody = rule.Description,
                AlertRulesId = rule.Id.ToString()
            };

            return alert;
        }

    }

}
