using System;

namespace Quze.Models.Models.ViewModels
{
    public class TimeTableExceptionVM : BaseVM
    {
        public int TimeTableId { get; set; }
        public int ExceptionType { get; set; }

        public static int Working { get;} = 2;
        public static int Late { get; } = 3;

        public DateTime DateTime { get; set; }
        public TimeSpan BeginTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
