using System;
using System.Collections.Generic;
using System.Linq;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;

namespace Quze.DAL
{
   public class TimeTableLogic
    {
        private readonly List<TimeTable> timeTables;
        private readonly List<TimeTableException> exceptions;
        private readonly List<TimeTableVacation> vacations;
        private readonly List<ServiceQueue> serviceQueuesSource;
        private readonly int maxServiceQ2FetchByTt;

        public TimeTableLogic(IEnumerable<TimeTable> timeTables, IQueryable<TimeTableException> exceptions, IQueryable<TimeTableVacation> vacations, int maxServiceQ2FetchByTt)
        {
            this.timeTables = timeTables.ToList();
            this.exceptions = exceptions.ToList();
            this.vacations = vacations.ToList();
            this.maxServiceQ2FetchByTt = maxServiceQ2FetchByTt;
        }

        public TimeTableLogic(IEnumerable<TimeTable> timeTables, IQueryable<TimeTableException> exceptions, IQueryable<TimeTableVacation> vacations, int maxServiceQ2FetchByTt, IQueryable<ServiceQueue> serviceQueuesSource)
        {
            this.timeTables = timeTables.ToList();
            this.exceptions = exceptions.ToList();
            this.vacations = vacations.ToList();
            this.maxServiceQ2FetchByTt = maxServiceQ2FetchByTt;
            this.serviceQueuesSource = serviceQueuesSource.ToList();
        }

        /// <summary>
        /// for a single time table Line
        /// </summary>
        /// <param name="ttl">time table line</param>
        /// <param name="date"></param>
        /// <returns>a new service queue</returns>
        private ServiceQueue CreateServiceQueueByTtLineIfNotExist(TimeTableLine ttl, DateTime date)
        {
            var newServiceQueue = new ServiceQueue(ttl, date);

            // if the serviceQueue already exist
            if (serviceQueuesSource.IsNotNullOrEmpty() &&
                serviceQueuesSource.Exists(x => x.Equals(newServiceQueue)))
                return null;
            // if in this date service provider in vacation 
            if (ttl.TimeTable.ServiceProvider.IsVacation(vacations, date))
                return null;

            var exception = ttl.TimeTable.FirstOrDefaultTtException(date, exceptions);
            var result = exception != null ? new ServiceQueue(exception) : newServiceQueue;
            return result;
        }

        private IEnumerable<ServiceQueue> CreateSQsInRangeOfDatesByTt(DateTime fromDate, DateTime toDate,
            TimeTable timeTable, int maxSq2Fetch)
        {
            var numOfServiceQueues = 0;
            var date = fromDate;
            for (; date <= toDate && numOfServiceQueues< maxSq2Fetch; date = date.AddDays(1), numOfServiceQueues++)
            {
                var ttl = timeTable.TimeTableLines.FirstOrDefault(x => x.WeekDay == date.QuzeDayOfWeek());
                if(ttl != null)
                    yield return CreateServiceQueueByTtLineIfNotExist(ttl, date);
            }
        }



        public  IEnumerable<ServiceQueue> CreateSQsInRangeOfDatesByTTs(DateTime fromDate, DateTime toDate)
        {
           // return  timeTables.SelectMany(timeTable => 
           //     CreateSQsInRangeOfDatesByTt(fromDate, toDate, timeTable, maxServiceQ2FetchByTt)
            //    );
           var serviceQueues = new List<ServiceQueue>();
            foreach (var timeTable in timeTables)
                serviceQueues.AddRange(CreateSQsInRangeOfDatesByTt(fromDate, toDate, timeTable, maxServiceQ2FetchByTt));

            return serviceQueues;
        }

        public IEnumerable<ServiceQueue> CreateSQsInRangeOfDatesByTTs(DateTime fromDate)
        {
           
            var serviceQueues = new List<ServiceQueue>();
            foreach (var timeTable in timeTables)
                serviceQueues.AddRange(CreateSQsInRangeOfDatesByTt(fromDate, timeTable.ValidUntilDate, timeTable, maxServiceQ2FetchByTt));

            return serviceQueues;
        }


    }
}

