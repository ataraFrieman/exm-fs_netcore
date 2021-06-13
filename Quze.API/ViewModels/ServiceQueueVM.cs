using System;
using System.Collections.Generic;
using Quze.Models.Models.ViewModels;

namespace Quze.API.ViewModels
{
    public class ServiceQueueVm : BaseVM
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ServideProviderId { get; set; }
        public int BranchId { get; set; }
        public DateTime? ActualBeginTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int CurrentAppointementId { get; set; }
        public List<AppointmentVm> Appointments { get; set; }
        public bool Passed { get; set; }
    }
}