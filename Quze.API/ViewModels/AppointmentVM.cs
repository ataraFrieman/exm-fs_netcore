using System;
using Quze.Models.Models.ViewModels;

namespace Quze.API.ViewModels
{
    public class AppointmentVm : BaseVM
    {

        public int? ServiceQueueId { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int BranchId { get; set; }

        public int FellowId { get; set; }

        /// <summary>
        /// Estimated
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// Estimated
        /// </summary>
        public DateTime? EndTime { get; set; }
        public DateTime? ActualBeginTime { get; set; }
        public int? ActualDuration { get; set; }
        public int? Duration { get; set; }

    }
}
