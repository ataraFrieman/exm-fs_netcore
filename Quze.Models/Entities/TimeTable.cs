using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Types;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Quze.Models.Entities
{
    public class TimeTable : EntityBase
    {
        public TimeTable()
        {
            TimeTableLines = new List<TimeTableLine>();
        }

        public int ServiceProviderId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int BranchId { get; set; }

        public DateTime ValidFromDate { get; set; }
        public DateTime ValidUntilDate { get; set; }
        public List<TimeTableLine> TimeTableLines { get; set; }

        public Branch Branch { get; set; }
        public ServiceType ServiceType { get; set; }
        
       
        public ServiceProvider ServiceProvider { get; set; }
      //  public int ServiceTypeId { get; set; }
        //public int OrganizationId { get; set; }
        public override string ToString()
        {
            var text = base.ToString();
            text += Environment.NewLine + $"ProviderId: {ServiceProviderId}, BranchId: {BranchId}, serviceTypeId: {ServiceTypeId} ";

            foreach (var item in TimeTableLines)
                text += Environment.NewLine + item;

            return text;
        }

        public void OrderTableLinesByNearestDateTime(DateTime dateTime)
        {
            TimeTableLines = TimeTableLines.OrderBy(l => l.NearestDateTime(dateTime)).ToList();
        }

        public void AddTimeTableLine(TimeTableLine line)
        {
            if (TimeTableLines.IsNull()) TimeTableLines = new List<TimeTableLine>();
            TimeTableLines.Add(line);
        }

        public void AddTimeTableLine(QuzeDayOfWeek weekDay, TimeSpan beginTime, TimeSpan endTime)
        {
            var line = new TimeTableLine(weekDay, beginTime, endTime);
            AddTimeTableLine(line);
        }


        /// <summary>
        /// Return lines that belong to the given week day
        /// </summary>                  
        /// <param name="weekDay"></param>
        /// <returns></returns>
        public List<TimeTableLine> this[QuzeDayOfWeek weekDay] =>
            TimeTableLines
            .Where(l => l.WeekDay == weekDay)
            .ToList();

        public bool CanSetApoointmentToday(QuzeDayOfWeek weekDay)
        {
            return TimeTableLines.Any(x => x.WeekDay == weekDay);
        }


        /// <summary>
        /// Gets actual line for the given date and time parameter
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public TimeTableLine this[DateTime dateTime]
        {
            get
            {
                OrderTableLinesByNearestDateTime(dateTime);
                var ttl = this[dateTime.QuzeDayOfWeek()]
                    .FirstOrDefault(line =>
                    dateTime.IsTimeBetween(line.BeginTime, line.EndTime)) ;
                ttl.TimeTable = this;
                return ttl;
            }
        }


    }
}
