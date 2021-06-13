using System;

namespace Quze.Models.Entities
{
    public class AppointmentTask: EntityBase
    {
        public int AppointmentId { get; set; }
        public int RequiredTaskId { get; set; }
        public RequiredTask RequiredTask { get; set; }
        public DateTime? LastReminderSentTime { get; internal set; }
        public DateTime? TimeApproved { get; set; }
        public string ApprovedFrom { get; set; }
        public bool? Approved { get; set; }
    }
}