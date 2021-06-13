using System;
using System.Collections.Generic;
using Quze.BL.UserQueue.UserConstraint;
using Quze.BL.Utiles;
using Quze.Models.Entities;
using Quze.Models.slots;

namespace Quze.BL.ServiceProviderSecretary
{
    public class ServiceQueueLogic
    {
        private List<SlotBase> iSlots;
        private ServiceQueue serviceQueue;
        private IEnumerable<PossibleTime> possibleTimes;

        public ServiceQueueLogic(ServiceQAndDateCreteria sqWithDatesCriteria)
        {
            serviceQueue = sqWithDatesCriteria.ServiceQueue;
            possibleTimes = sqWithDatesCriteria.PossibleTimes;
            iSlots = new List<SlotBase>();
        }


        public IEnumerable<EmptySlot> GetAvailableSlots()
        {
            throw new NotImplementedException();
            
        }
    }
}
