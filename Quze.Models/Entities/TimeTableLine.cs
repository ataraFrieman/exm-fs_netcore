using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Types;
using System;

namespace Quze.Models.Entities
{
    public class TimeTableLine : EntityBase
    {
        public TimeTableLine()
        {

        }

        public TimeTableLine(QuzeDayOfWeek weekDay, TimeSpan beginTime, TimeSpan endTime)
        {
            WeekDay = weekDay;
            SetBeginTime(beginTime);
            SetEndTime(endTime);
        }

        public int TimeTableId { get; set; }
        //public int ServiceProviderId { get; set; }
        //public int OrganizationId { get; set; }
        //public int BranchId { get; set; }
        //public DateTime ValidFromDate { get; set; }
        //public DateTime ValidUntilDate { get; set; }
        public QuzeDayOfWeek WeekDay { get; set; }
        public TimeSpan BeginTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeTable TimeTable { get; set; }

        public void SetBeginTime(TimeSpan time)
        {
            BeginTime = new TimeSpan(time.Hours, time.Minutes, 0);
        }

        public void SetEndTime(TimeSpan time)
        {
            EndTime = new TimeSpan(time.Hours, time.Minutes, 0);
        }

        public override string ToString()
        {
            var str = string.Format("at {0} between {1} and {2}", WeekDay.ToString(), BeginTime.ToString(@"hh\:mm"), EndTime.ToString(@"hh\:mm"));
            return str;
        }


        /// <summary>
        /// returns the closest future date and time compare to the dateTime parameter that SP will start serving according to the current TimeTableLine
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public DateTime NearestDateTime(DateTime? dateTime)
        {
            if (dateTime.IsNull())
                dateTime = DateTime.Now;
            var nextDateTime= dateTime.Value.GetNextDayOfWeek(WeekDay);
            if (nextDateTime.Date == DateTime.Today && dateTime.Value.TimeOfDay > EndTime)
            {
                dateTime = DateTime.Today.AddDays(7);
            }
            //nextDateTime = dateTime.Value.AddSeconds(BeginTime.TotalSeconds);
            return nextDateTime;
        }
    }
}
