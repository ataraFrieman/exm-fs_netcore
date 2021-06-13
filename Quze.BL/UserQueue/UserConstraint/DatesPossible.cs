using System;
using System.Collections.Generic;

namespace Quze.BL.UserQueue.UserConstraint
{
   public class DatesPossible
    {
        public List<PossibleTime> possibleTimes;
        public DatesPossible(List<PossibleTime> possibleTimes)
        {
           this.possibleTimes = possibleTimes;
        }

        public void add(PossibleTime possibleTime)
        {
            possibleTimes.Add(possibleTime);
        }




        //// return begin date of the period
        //public DateTime BeginDate { get; set; }
        //// return end date of the period
        //public DateTime EndDate { get; set; }

        ////returns begin time for every day in period specified
        //public DateTime BeginTime { get; set; }
        ////returns begin time for every day in period specified
        //public DateTime EndTime { get; set; }

        public override string ToString()
        {
            var result = "possibleTimes: ";
            foreach (var possibleTime in possibleTimes)
            {
                result += " , " + possibleTime;
            }
            return result;
        }
    }


    public class PossibleTime
    {
        public PossibleTime(DateTime begin,DateTime end)
        {
            Begin = begin;
            End = end;
        }

        public int Id { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public override string ToString()
        {
            return $"id: {Id} Begin: {Begin} End: {End}";
        }
    }
}
