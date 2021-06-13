using System;

namespace Quze.Models.Logic
{
    public class Time
    {
      
        private static DateTime now=DateTime.Now;
        public static DateTime SetNow(TimeSpan time) {
            now = now.Date + time;
            return now;
        }
        public static DateTime GetNow()
        {
            //return now;
            return DateTime.Now;
        }
        public static DateTime GetNow(DateTime customDate) {
           now= customDate;
            return  now;
        }
        public static DateTime GetNow(int hours,int minutes)
        {
            TimeSpan time = new TimeSpan(hours, minutes, 0);
            return  now.Date+ time;
        }
    }
}
