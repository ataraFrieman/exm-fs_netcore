using System.Collections.Generic;
using System.Linq;
using Quze.BL.UserQueue.UserConstraint;
using Quze.BL.Utiles;
using Quze.Models.Entities;

namespace Quze.BL.QueuingManagement
{
    class MatchingSQsWithDates
    {
        private DatesPossible datesPossible;
        private List<ServiceQueue> serviceQueuesSource;
        

        public MatchingSQsWithDates(DatesPossible datesPossible, IEnumerable<ServiceQueue> serviceQueuesSource)
        {
            this.datesPossible = datesPossible;
            this.serviceQueuesSource = serviceQueuesSource.ToList();
        }

        public IEnumerable<ServiceQAndDateCreteria> GetDatesMatchedWithSqs()
        {
            //var datesMatchWithSqList = new List<DatesMatchWithSq>();
            //var result =
            //    from sq in serviceQueuesSource
            //    from timePossible in datesPossible.possibleTimes
            //    where sq.BeginTime.Date == timePossible.Begin.Date
            //    select new DatesMatchWithSq()
            var result = new List<ServiceQAndDateCreteria>();

            foreach (var serviceQueue in serviceQueuesSource)
            {
                if (!datesPossible.possibleTimes.Exists(x => x.Begin.Date == serviceQueue.BeginTime.Date))
                    continue;

                    var dates = datesPossible.possibleTimes.Where(x => x.Begin.Date == serviceQueue.BeginTime.Date);
                    ServiceQAndDateCreteria datesMatchWithSq = new ServiceQAndDateCreteria(dates, serviceQueue);
                    result.Add(datesMatchWithSq);
            }

            return result;
        }


    }
}
