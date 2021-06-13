using System.Collections.Generic;
using System.Linq;
using Quze.BL.UserQueue.UserConstraint;
using Quze.DAL;
using Quze.Models.Entities;
using Quze.Models.slots;

namespace Quze.BL.QueuingManagement
{
  public  class QueueManager
    {
        private readonly UserConstraints constraints;
        private IEnumerable<ServiceProvider> serviceProviders;
        private readonly List<ServiceProvidersServiceType> serviceProvidersServiceTypes;
        private List<ServiceQueue> serviceQueues;
        private List<TimeTable> timeTables;
        private IEnumerable<SlotBase> slots;
        private IEnumerable<TimeTableVacation> tableVacations;

        public QueueManager(UserConstraints constraints, IEnumerable<ServiceProvider> serviceProviders, List<ServiceProvidersServiceType> serviceProvidersServiceTypes, List<ServiceQueue> serviceQueues, List<TimeTable> timeTables, IEnumerable<TimeTableVacation> tableVacations)
        {
            slots = new List<SlotBase>();
            this.constraints = constraints;
            this.serviceProviders = serviceProviders;
            this.serviceProvidersServiceTypes = serviceProvidersServiceTypes;
            this.serviceQueues = serviceQueues;
            this.timeTables = timeTables;
            this.tableVacations = tableVacations;
        }

        public IEnumerable<TimeTable> GetRelevantTimeTables()
        {
            var result = timeTables
                .FilterByBranches(constraints.Branches)
                .FilterByServiceProviders(constraints.ServiceProviders)
                .FilterByServiceType(constraints.ServiceType);
            return result;
        }

        public void mainFunc()
        {
            var relevantTimeTables = GetRelevantTimeTables();

            foreach (var tt in relevantTimeTables)
            {
                slots = slots.Concat(getSlotsFromTimeTable(tt, constraints.DateConstraint));
            }

        }


        public IEnumerable<SlotBase> getSlotsFromTimeTable(TimeTable timeTable, DatesPossible datesPossibles)
        {
            var result = new List<SlotBase>();
            ServiceProvider serviceProvider = serviceProviders.FirstOrDefault(sp => sp.Id == timeTable.ServiceProviderId);
            foreach (var searchedRange in datesPossibles.possibleTimes)
            {
                // if (tim.All(serviceProvider.IsVacation(tableVacations,searchedRange.Begin));
                // serviceProvider.IsVacation(tableVacations, searchedRange.Begin)
            }


            //            ServiceQueueManager serviceQueueManager = new ServiceQueueManager(timeTable, );

            return result;
        }



        public IEnumerable<ServiceProvider> FilterServiceProvidersByConstraint(IEnumerable<ServiceProvider> sProviders)
        {
            var sp = serviceProviders
                .FilterByOrganization(constraints.Organization)
                .FilterByServiceType(constraints.ServiceType, serviceProvidersServiceTypes);
            return sp;
        }






    }
}
