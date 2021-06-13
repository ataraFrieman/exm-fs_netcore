using System;

namespace Quze.Infrastruture.Extensions
{
    public static class TimeSpanExtensions
    {
        public static bool IsBetween(this TimeSpan me, TimeSpan start, TimeSpan end)
        {
            return (me >= start && me <= end);
        }

        public static DateTime ToDateTime(this TimeSpan me)
        {
            return new DateTime(me.Ticks);
        }

        public static TimeSpan AddSeconds(this TimeSpan me, int seconds)
        {
            return me.Add(TimeSpan.FromSeconds(seconds));
        }

        public static int IntTotalSeconds(this TimeSpan me)
        {
            return (int)me.TotalSeconds;
        }

        /// <summary>
        /// Gets duration in seconds between this instance (the Start Time) to the endTime parametre
        /// </summary>
        /// <param name="startIme"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int GetDuration(this TimeSpan startIme,TimeSpan endTime)
        {
            return (endTime - startIme).IntTotalSeconds();
        }
    }
}
