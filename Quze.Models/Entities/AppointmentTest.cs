using System;

namespace Quze.Models.Entities
{
    public class AppointmentTest: EntityBase
    {
        public int AppointmentId { get; set; }
        public int RequiredTestId { get; set; }
        public RequiredTest RequiredTest { get; set; }
        public DateTime? LastReminderSentTime { get; internal set; }
        public DateTime? TimeApproved { get; set; }
        public string ApprovedFrom { get; set; }
        public bool? Approved { get; set; }
        public string ValueOfTest { get; set; } = null;
        //public int? ValueOfTest { get; set; }
    }
}
