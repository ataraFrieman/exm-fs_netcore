using System.Collections.Generic;
using Quze.BL.UserQueue.UserConstraint;
using Quze.DAL;
using Quze.Models.Entities;

namespace Quze.BL.QueuingManagement
{
   public class FilterTTByCriteria
    {
        private UserConstraints userConstraints;
        private IEnumerable<TimeTable> timeTables;

        public FilterTTByCriteria(UserConstraints userConstraints, IEnumerable<TimeTable> timeTables)
        {
            this.userConstraints = userConstraints;
            this.timeTables = timeTables;
        }

        public IEnumerable<TimeTable> GetRelevantTimeTables()
        {
            var result = timeTables
                .FilterByBranches(userConstraints.Branches)
                .FilterByServiceProviders(userConstraints.ServiceProviders)
                .FilterByServiceType(userConstraints.ServiceType);
            return result;
        }

        

    }
}
