using Quze.Infrastruture.Types;
using System;

namespace Quze.Models.Models.ViewModels
{
    public class TimeTableLineVM : BaseVM
    {
   

        public int TimeTableId { get; set; }
       
        public QuzeDayOfWeek WeekDay { get; set; }
        public TimeSpan BeginTime { get; set; }
        public TimeSpan EndTime { get; set; }

      
        public override string ToString()
        {
            var str = string.Format("at {0} between {1} and {2}", WeekDay.ToString(), BeginTime.ToString(@"hh\:mm"), EndTime.ToString(@"hh\:mm"));
            return str;
        }

    }
}
