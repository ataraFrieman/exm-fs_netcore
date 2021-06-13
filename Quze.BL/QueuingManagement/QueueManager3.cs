using System.Collections.Generic;
using System.Linq;
using Quze.BL.UserQueue.UserConstraint;
using Quze.BL.Utiles;
using Quze.DAL;
using Quze.Models.Entities;
using Quze.Models.slots;
using ServiceQueueLogic = Quze.BL.ServiceProviderSecretary.ServiceQueueLogic;


namespace Quze.BL.QueuingManagement
{
  public  class QueueManager3
    {
        private readonly UserConstraints userConstraints;
        private readonly IEnumerable<TimeTable> timeTablesSource;
        private readonly IEnumerable<TimeTableException> exceptions;
        private readonly IEnumerable<TimeTableVacation> vacations;
        private readonly IEnumerable<ServiceQueue> serviceQueuesSource;
        private readonly IEnumerable<ServiceProvider> serviceProviders;

        public QueueManager3(UserConstraints userConstraints, IEnumerable<TimeTable> timeTablesSource, IEnumerable<TimeTableException> exceptions, IEnumerable<TimeTableVacation> vacations, IEnumerable<ServiceQueue> serviceQueuesSource, IEnumerable<ServiceProvider> serviceProviders)
        {
            this.userConstraints = userConstraints;
            this.timeTablesSource = timeTablesSource;
            this.exceptions = exceptions;
            this.vacations = vacations;
            this.serviceQueuesSource = serviceQueuesSource;
            this.serviceProviders = serviceProviders;
        }

        public IEnumerable<TimeTable> FilterTimeTables()
        {
            var filterTtByCriteria = new FilterTTByCriteria(userConstraints, timeTablesSource);
            var result = filterTtByCriteria.GetRelevantTimeTables();
            return result;
        }

        public IEnumerable<ServiceQueue> GetNewServiceQueues(IEnumerable<TimeTable> timeTables)
        {
         //   var TTLogic = new TimeTableLogic(timeTables, exceptions, vacations,serviceProviders, serviceQueuesSource);
            IEnumerable<ServiceQueue> newServiceQueues = new List<ServiceQueue>();
            foreach (var dateRange in userConstraints.DateConstraint.possibleTimes)
            {
                // todo: finish
              //  newServiceQueues = newServiceQueues.Concat(
               //     TTLogic.CreateSQsInRangeOfDatesByTTs(dateRange.Begin, dateRange.End)
                 //   );
            }

            return newServiceQueues;
        }

        private IEnumerable<ServiceQAndDateCreteria> GetDatesMatchesWithSq(IEnumerable<ServiceQueue> serviceQueues)
        {
            var matchingSQsByDates = new MatchingSQsWithDates(userConstraints.DateConstraint, serviceQueues);
            var result = matchingSQsByDates.GetDatesMatchedWithSqs();
            return result;
        }

        private IEnumerable<SlotBase> GetSlotsBySqAndDateCreteria(ServiceQAndDateCreteria datesMatch)
        {
            
            var sqLogic = new ServiceQueueLogic(datesMatch);
            return sqLogic.GetAvailableSlots();
        }

        private IEnumerable<SlotBase> GetSlotsBySqAndDateCriteria(IEnumerable<ServiceQAndDateCreteria> datesMatches)
        {
            IEnumerable<SlotBase> slots = new List<SlotBase>();
            foreach (var dateMatch in datesMatches)
            {
                 var sqLogic = new ServiceQueueLogic(dateMatch);
                 slots =  slots.Concat(sqLogic.GetAvailableSlots());
            }

            return slots;
        }


        public void Run()
        {
            var filteredTimeTables = FilterTimeTables();
            var generatedServiceQueues = GetNewServiceQueues(filteredTimeTables);
            var allServiceQueues = serviceQueuesSource.Concat(generatedServiceQueues);
            var sqWithDatesMatched = GetDatesMatchesWithSq(allServiceQueues);
            var slots = GetSlotsBySqAndDateCriteria(sqWithDatesMatched);
        }






    }
}
