using Quze.Models.Entities;
using System;
using System.Collections.Generic;

namespace CreateAlerts.Test
{
    public class InitAppointmentList
    {
        public Appointment initAppointmentWithoutAlerts()
        {
            var initApointmentTasksAndDocs = new InitAppointmentTaskAndDocuments();
            var result = new List<Appointment>();
            return new Appointment()
            {
                Id = 123,
                ServiceQueueId = 12,
                FellowId = 12,
                ServiceTypeId = 12,

                BeginTime = new DateTime(2019, 01, 02, 10, 00, 00),
                EndTime = new DateTime(2019, 01, 02, 10, 30, 00),
                Duration = 1800,
                AppointmentTasks = new List<AppointmentTask>()
                {
                    initApointmentTasksAndDocs.GetAppointmentTask1()
                },
                AppointmentDocs = new List<AppointmentDocument>()
                {
                    initApointmentTasksAndDocs.GetAppointmentDocs1()
                }
            };
        }

        public Appointment initAppointmentWithAlerts()
        {
            var initApointmentTasksAndDocs = new InitAppointmentTaskAndDocuments();
            var result = new List<Appointment>();
            return new Appointment()
            {
                Id = 124,
                ServiceQueueId = 12,
                FellowId = 12,
                ServiceTypeId = 12,

                BeginTime = new DateTime(2019, 01, 02, 10, 00, 00),
                EndTime = new DateTime(2019, 01, 02, 10, 30, 00),
                Duration = 1800,
                AppointmentTasks = new List<AppointmentTask>()
                {
                   initApointmentTasksAndDocs.GetAppointmentTask1()
                },
                AppointmentDocs = new List<AppointmentDocument>()
                {
                    initApointmentTasksAndDocs.GetAppointmentDocs1()
                },
                //Alerts = new List<Alert>()
                //{
                //    new Alert()
                //    {
                //        AlertRulesId  = "123",
                //        AlertType = AlertType.SMS,
                //        To  = "0585814444",
                //        MessageTitle ="",
                //        MessageBody ="",
                //        ActualSentTime = new DateTime(2019,1,1,10,0,0)
                //    }
                //}
            };
        }
    }
}
