using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quze.CreateAlerts
{
    public class CreateAppointmentsAlert
    {
        private List<Appointment> appointments;
        DateTime now;

        public CreateAppointmentsAlert(List<Appointment> appointments, DateTime? now = null)
        {
            this.appointments = appointments;
            this.now = now.IsNull() ? DateTime.Now : now.Value;
        }

        public IEnumerable<Alert> GetAllAppointmentsAlerts()
        {
            var alertsBag = new ConcurrentBag<Alert>();

            Parallel.ForEach(appointments, (appointment) =>
            {
                var createAppointmentAlert = new CreateAppointmentAlerts(appointment, now);
                var list = createAppointmentAlert.GetAppointmentAlerts();
                alertsBag.AddRange(list);
            });
            return alertsBag;
        }
    }

}
