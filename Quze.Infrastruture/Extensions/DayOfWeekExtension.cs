using Quze.Infrastruture.Types;
using System;

namespace Quze.Infrastruture.Extensions
{
    public static class DayOfWeekExtensions
    {
        public static QuzeDayOfWeek ToQuzeDayOfWeek(this DayOfWeek me)
        {
            return (QuzeDayOfWeek)((int)(me) + 1);
        }
    }
}
