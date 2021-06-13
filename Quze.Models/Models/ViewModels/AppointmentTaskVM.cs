using System;

namespace Quze.Models.Models.ViewModels
{
    public class AppointmentTaskVM: BaseVM
    {
        public int AppointmentId { get; set; }
        public int RequiredTaskId { get; set; }
        public RequiredTaskVM RequiredTask { get; set; }
        public DateTime? TimeApproved { get; set; }
        public string ApprovedFrom { get; set; }
        
    }
}