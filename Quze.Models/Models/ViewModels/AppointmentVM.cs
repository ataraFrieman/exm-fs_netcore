using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Quze.Models.Entities;

namespace Quze.Models.Models.ViewModels
{
    public class AppointmentVM : BaseVM
    {
        public int ServiceQueueId { get; set; }
        public int FellowId { get; set; }
        public int? ServiceTypeId { get; set; }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? Duration { get; set; }

        public DateTime? ActualBeginTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int? ActualDuration { get; set; }

        public Boolean Served { get; set; }
        public int? NoShow { get; set; }

        public virtual ServiceQueue ServiceQueue { get; set; } = null;
        public FellowVM Fellow { get; set; }
        public virtual ServiceType ServiceType { get; set; } = null;
        public virtual List<AppointmentTask> AppointmentTasks { get; set; }
        public virtual List<AppointmentDocument> AppointmentDocs { get; set; }

        public DateTime? reminderSent { get; set; }
        public DateTime? reminderReceived { get; set; }

    }
}
