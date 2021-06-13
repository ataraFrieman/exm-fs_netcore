using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{
    public class Conflict : EntityBase
    {
        public int AppointmentAId { get; set; }
        public int AppointmentBId { get; set; }
        public Appointment AppointmentA { get; set; }
        public Appointment AppointmentB { get; set; }
        public string Type { get; set; }
        public DateTime ConflictBeginTime { get; set; }
        public DateTime ConflictEndTime { get; set; }
        public int ServiceQueueId { get; set; }
        
    }
}
