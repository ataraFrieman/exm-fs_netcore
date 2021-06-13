using Quze.Models.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class AppointmentTaskVM : BaseVM
    {
        public int AppointmentId { get; set; }
        public int RequiredTaskId { get; set; }
        public RequiredTaskVM RequiredTask { get; set; }
        public DateTime? LastReminderSentTime { get; internal set; }
        public DateTime? TimeApproved { get; set; }
        public string ApprovedFrom { get; set; }
        public bool? Approved { get; set; }
    }
}
