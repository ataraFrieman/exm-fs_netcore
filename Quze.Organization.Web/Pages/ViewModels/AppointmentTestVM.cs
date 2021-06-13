using Quze.Models.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class AppointmentTestVM : BaseVM
    {
        public int AppointmentId { get; set; }
        public int RequiredTestId { get; set; }
        public RequiredTestVM RequiredTest { get; set; }
        public DateTime? LastReminderSentTime { get; internal set; }
        public DateTime? TimeApproved { get; set; }
        public string ApprovedFrom { get; set; }
        public bool? Approved { get; set; }
        public string ValueOfTest { get; set; } = null;
        //public int? ValueOfTest { get; set; }
    }
}
