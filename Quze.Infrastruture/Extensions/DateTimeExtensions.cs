using Quze.Infrastruture.Types;
using System;

namespace Quze.Infrastruture.Extensions
{
    public enum TimeInDay { Morning, AfterNoon, Evening }

    public static class DateTimeExtensions
    {
        private static TimeSpan morning = new TimeSpan(12, 00, 00);
        private static TimeSpan afterNoon = new TimeSpan(16, 00, 00);

        public static QuzeDayOfWeek QuzeDayOfWeek(this DateTime me)
        {
            return (QuzeDayOfWeek)(((int)me.DayOfWeek) + 1);
        }

        public static DateTime ChangeTime(this DateTime me, TimeSpan time)
        {
            me = me.Date;
            me = me.Add(time);
            return me;
        }

        public static bool IsTomorrow(this DateTime me)
        {
            return (DateTime.Today - me.Date).TotalDays == -1;
        }

        public static TimeInDay GetTimeInDay(this DateTime dt)
        {
            if (dt.TimeOfDay < morning)
            {
                return TimeInDay.Morning;
            }
            else if (dt.TimeOfDay < afterNoon)
            {
                return TimeInDay.AfterNoon;
            }
           
            return TimeInDay.Evening;
        }
        
        public static DateTime ZeroMilliseconds(this DateTime dt)
        {
            return new DateTime(((dt.Ticks / 10000000) * 10000000), dt.Kind);
        }


        /// <summary>
        /// Gets a date value that represents the next date that the given weekDay parameter occures
        /// </summary>
        /// <param name="me"></param>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        public static DateTime GetNextDayOfWeek(this DateTime me, QuzeDayOfWeek weekDay)
        {
            var meDateNumber = (int)me.QuzeDayOfWeek();
            var weekDayNumber = (int)weekDay;
            var daysToAdd = meDateNumber > weekDayNumber ? 7 : 0;
            daysToAdd += weekDayNumber - meDateNumber;
            var date = me.AddDays(daysToAdd);
            return date;

            //for (var date = me; date < date.AddDays(8); date.AddDays(1))
            //    if (date.QuzeDayOfWeek() == weekDay)
            //        return date;
            //return new DateTime();
        }

        public static bool IsBetween(this DateTime me, DateTime start, DateTime end)
        {
            return (me >= start && me <= end);
        }

        public static bool IsTimeBetween(this DateTime me, TimeSpan start, TimeSpan end)
        {
            return (me.TimeOfDay >= start && me.TimeOfDay <= end);
        }


        public static TimeSpan ToTimeSpan(this DateTime me)
        {
            return TimeSpan.FromTicks(DateTime.Now.Ticks);
        }

    }
}
