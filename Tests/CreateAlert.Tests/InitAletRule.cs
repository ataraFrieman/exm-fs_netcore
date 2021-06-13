using Quze.Models.Entities;
using Quze.Models.Models.Alerts;
using System.Collections.Generic;

namespace CreateAlerts.Test
{
    class InitAletRule
    {
        public AlertRule GetAlertRule1()
        {
            return new AlertRule()
            {
                Id = 123,
                AlertTypeID = AlertType.SMS,
                RequiredTaskID = 1,
                Description = "על מנת להתכונן לתור עליך להפסיק לאכול בעוד שעתיים",
                HoursBeforeToAlert = 4,
                RepeatInterval = 1
            };
        }

        public AlertRule GetAlertRule2()
        {
            return new AlertRule()
            {
                Id = 111,
                AlertTypeID = AlertType.Email,
                RequiredTaskID = 1,
                Description = "על מנת להתכונן לתור עליך לצום מחר החל מ8:00",
                HoursBeforeToAlert = 24,
                RepeatInterval = 1
            };
        }

        public AlertRule GetAlertRule3()
        {
            return new AlertRule()
            {
                Id = 222,
                AlertTypeID = AlertType.Email,
                RequiredDocumentID = 1,
                Description = "יש להעלות טופס 17",
                HoursBeforeToAlert = 240,
                RepeatInterval = 24
            };
        }

        public AlertRule GetAlertRule4()
        {
            return new AlertRule()
            {
                Id = 333,
                AlertTypeID = AlertType.SMS,
                RequiredDocumentID = 1,
                Description = "עדיין לא העלית טופס 17",
                HoursBeforeToAlert = 48,
                RepeatInterval = 24
            };
        }

        public List<AlertRule> GetAlertRulesTaskList()
        {
            return new List<AlertRule>()
            {
                GetAlertRule1(),GetAlertRule2()
            };
        }

        public List<AlertRule> GetAlertRulesDocumentList()
        {
            return new List<AlertRule>()
            {
                GetAlertRule3(),GetAlertRule4()
            };
        }
    }
}
