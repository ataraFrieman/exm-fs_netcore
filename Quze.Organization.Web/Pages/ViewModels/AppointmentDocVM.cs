using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quze.Models.Models.Alerts;
using Quze.Models.Models.ViewModels;

namespace Quze.Organization.Web.ViewModels
{
    public class AppointmentDocVM : BaseVM
    {
        public int AppointmentId { get; set; }
        public int RequiredDocumentId { get; set; }
        public RequiredDocumentVM RequiredDocument { get; set; }
        public DateTime? LastReminderSentTime { get; set; }
        public DateTime? TimeApproved { get; set; }
        public string ApprovedFrom { get; set; }
        public int? DocumentContentId { get; set; }
        public bool? Approved { get; set; }
        public string FileContent { get; set; }
    }
}
