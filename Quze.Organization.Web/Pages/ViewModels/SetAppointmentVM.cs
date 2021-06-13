using System;
using System.Collections.Generic;

namespace Quze.Organization.Web.ViewModels
{
    public class SetAppointmentVM : BaseVM
    {
        public int? ServiceProviderId { get; set; }
        public string FellowIdentityNumber { get; set; }
        public int BranchId { get; set; }
        public int? ServiceQueueId { get; set; }
        public int ServiceTypeId { get; set; }
        public DateTime BeginTime { get; set; }
        public string PhoneNumber { get; set; }
    }
}
