using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{
    public class AllocationOfEquipment:EntityBase
    {
        public int? EqpId { get; set; }
        public int? AppointmentId { get; set; }
        public int? Amount { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Equipment Equipment { get; set; } = null;
        public Appointment Appointment { get; set; } = null;



    }
}
