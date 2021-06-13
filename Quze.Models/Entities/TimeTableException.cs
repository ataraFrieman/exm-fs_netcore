using System;

namespace Quze.Models.Entities
{
    public class TimeTableException : EntityBase
    {
        public int TimeTableId { get; set; }
        public TimeTable TimeTable { get; set; }
        public int ExceptionType { get; set; }

        public static int Working { get;} = 2;
        public static int Late { get; } = 3;

        //public DateTime Date { get; set; }

        public DateTime DateTime { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
