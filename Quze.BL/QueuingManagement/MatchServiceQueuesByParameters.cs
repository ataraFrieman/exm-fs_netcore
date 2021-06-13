using System.Collections.Generic;
using System.Linq;
using Quze.BL.UserQueue.UserConstraint;
using Quze.DAL;
using Quze.Models.Entities;

namespace Quze.BL.QueuingManagement
{
    class MatchServiceQueuesByParameters
    {
        private List<ServiceQueue> serviceQueuesSource;
        private UserConstraints constraints;

        public MatchServiceQueuesByParameters(IEnumerable<ServiceQueue> serviceQueuesSource, UserConstraints constraints)
        {
            this.serviceQueuesSource = serviceQueuesSource.ToList();
            this.constraints = constraints;
        }

        //public IEnumerable<ServiceQueue> MutchedServiceQueues
        //{
        //    get
        //    {
        //        var filteredServiceQueues = serviceQueuesSource
        //            .FilterByBranches(constraints.Branches)
        //            .FilterByServiceType(constraints.ServiceType)
        //            .FilterByServiceProviders(constraints.ServiceProviders);
        //        return filteredServiceQueues;
        //    }
        //}
    }
}
