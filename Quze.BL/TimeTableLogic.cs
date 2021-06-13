//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Quze.DAL;
//using Quze.Infrastruture.Extensions;
//using Quze.Models.Entities;

//namespace Quze.BL
//{
//    internal class TimeTableLogic
//    {
//        private readonly IEnumerable<TimeTable> timeTables;
//        private readonly IQueryable<TimeTableException> exceptions;
//        private readonly IQueryable<TimeTableVacation> vacations;
//        private readonly IEnumerable<ServiceQueue> existingServiceQueues;

//        public TimeTableLogic(IEnumerable<TimeTable> timeTables, IQueryable<TimeTableException> exceptions, IQueryable<TimeTableVacation> vacations, IEnumerable<ServiceQueue> existingServiceQueues)
//        {
//            this.timeTables = timeTables;
//            this.exceptions = exceptions;
//            this.vacations = vacations;
//            this.existingServiceQueues = existingServiceQueues;
//        }

//        /// <summary>
//        /// for a single time table Line
//        /// </summary>
//        /// <param name="ttl">time table line</param>
//        /// <param name="date"></param>
//        /// <returns>a new service queue</returns>
//        private ServiceQueue CreateServiceQueueByTtLineIfNotExist(TimeTableLine ttl, DateTime date)
//        {
//            var newServiceQueue = new ServiceQueue(ttl, date);

//            // if in this date service provider in vacation 
//            if (ttl.TimeTable.ServiceProvider.IsVacation(vacations, date))
//                return null;

//            if (existingServiceQueues.Any(sq => sq.ServiceProviderId == newServiceQueue.ServiceProviderId
//                                                   && sq.BeginTime == newServiceQueue.BeginTime
//                                                   && sq.EndTime == newServiceQueue.EndTime))
//                return null;

           
//            var exception = ttl.TimeTable.FirstOrDefaultTtException(date, exceptions);
//            var result = exception != null ? new ServiceQueue(exception) : newServiceQueue;
//            return result;
//        }

//        private IEnumerable<ServiceQueue> CreateSQsInRangeOfDatesByTt(DateTime fromDate, DateTime toDate,
//            TimeTable timeTable)
//        {
//            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
//            {
//                var ttl = timeTable.TimeTableLines.FirstOrDefault(x => x.WeekDay == date.QuzeDayOfWeek());
//                yield return  CreateServiceQueueByTtLineIfNotExist(ttl, date);
//            }
//        }



//        public IEnumerable<ServiceQueue> CreateSQsInRangeOfDatesByTTs(DateTime fromDate, DateTime toDate)
//        {
//            var result = new List<ServiceQueue>();
//            foreach (var timeTable in timeTables)
//            {
//               result.AddRange(CreateSQsInRangeOfDatesByTt(fromDate, toDate, timeTable));    
//            }

//            return result;
//        }
        

//    }
//}
