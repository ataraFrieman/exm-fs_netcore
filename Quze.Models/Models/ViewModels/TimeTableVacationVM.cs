using System;

namespace Quze.Models.Models.ViewModels
{
    public class TimeTableVacationVM : BaseVM
    {
        public int ServiceProviderId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
