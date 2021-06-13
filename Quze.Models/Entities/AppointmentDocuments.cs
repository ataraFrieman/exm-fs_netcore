using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    public class AppointmentDocument : EntityBase
    {
        public int AppointmentId { get; set; }
        public int RequiredDocumentId { get; set; }
        public RequiredDocument RequiredDocument { get; set; }
        public DateTime? LastReminderSentTime { get; set; }
        public DateTime? TimeApproved { get; set; }
        public string ApprovedFrom { get; set; }
        public int? DocumentContentId { get; set; }
        public bool? Approved { get; set; }
        [NotMapped]
        public string FileContent { get; set; }
    }
}