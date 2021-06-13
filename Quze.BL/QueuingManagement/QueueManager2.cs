using System.Collections.Generic;
using System.Linq;
using Quze.BL.ServiceProviderSecretary;
using Quze.BL.UserQueue.UserConstraint;
using Quze.BL.Utiles;
using Quze.Models.Entities;
using Quze.Models.slots;

namespace Quze.BL.QueuingManagement
{
    class QueueManager2
    {
        private UserConstraints constraints;
        private List<ServiceQueue> serviceQueuesSource;
        private MatchServiceQueuesByParameters matchServices;


        public QueueManager2(UserConstraints constraints, List<ServiceQueue> serviceQueuesSource)
        {
            this.constraints = constraints;
            this.serviceQueuesSource = serviceQueuesSource;
        }

        private IEnumerable<ServiceQueue> GetMatchSQsByUserConstraints(IEnumerable<ServiceQueue> serviceQueues, UserConstraints constraints)
        {
            matchServices = new MatchServiceQueuesByParameters(serviceQueues, constraints);
        //    var result = matchServices.MutchedServiceQueues;
        //    return result;
        return null;
        }

        private IEnumerable<ServiceQAndDateCreteria> GetDatesMatchesWithSq(IEnumerable<ServiceQueue> serviceQueues)
        {
            var matchingSQsByDates = new MatchingSQsWithDates(constraints.DateConstraint, serviceQueues);
         var result =   matchingSQsByDates.GetDatesMatchedWithSqs();
         return result;
        }

        private IEnumerable<SlotBase> GetRelevantTimeRanges(ServiceQAndDateCreteria datesMatch)
        {
            var a = new ServiceQueueLogic(datesMatch);
            return a.GetAvailableSlots();
        }

        public List<SlotBase> GetsSlots()
        {
          var SqFilteredByConstraints =  GetMatchSQsByUserConstraints(serviceQueuesSource, constraints);
          var datesMatchedWithServiceQueue = GetDatesMatchesWithSq(SqFilteredByConstraints);
          var result = new List<SlotBase>();

          foreach (var time in datesMatchedWithServiceQueue)
          {
              var a = GetRelevantTimeRanges(time).ToList();
              result.AddRange(a);
          }

          return result;
        }
    }
}
