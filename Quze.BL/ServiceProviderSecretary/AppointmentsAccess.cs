using System;
using System.Collections.Generic;
using System.Linq;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Models.slots;

namespace Quze.BL.ServiceProviderSecretary
{
    public class AppointmentsAccess
    {
        private List<ISlot> slots;
        private ServiceQueue serviceQueue;

        public AppointmentsAccess(ServiceQueue serviceQueue)
        {
            this.serviceQueue = serviceQueue;
            slots = new List<ISlot>
            {
                new BeginOfSlots(serviceQueue.BeginTime, serviceQueue.BeginTime),
                new EndOfSlots(serviceQueue.EndTime, serviceQueue.EndTime)
            };

            if (serviceQueue.Appointments.IsNullOrEmpty())
                slots.Add(new EmptySlot(serviceQueue.BeginTime, serviceQueue.EndTime));
          //  else
              //  slots.AddRange(serviceQueue.Appointments);


        }




       

        public bool IsPossibleToInsert(Slot newSlot, ISlot hole)
        {
            // var hole = GetSlotByTime(newSlot.BeginTime);
            if (!(hole is EmptySlot))
                return false;

            return !newSlot.BeginTime.IsBetween(hole.BeginTime, hole.EndTime) || !newSlot.EndTime.IsBetween(hole.BeginTime, hole.EndTime);
        }

        public void AddSlot(Slot newSlot)
        {
            var hole = GetSlotByTime(newSlot.BeginTime);

            if (!IsPossibleToInsert(newSlot, hole))
                throw new Exception("is impossible to add the range");

            var emptySlot = hole as EmptySlot;
            slots.Remove(hole);
            slots.AddRange(SplitEmptySlot(emptySlot, newSlot));
        }

        private IEnumerable<SlotBase> SplitEmptySlot(EmptySlot currentEmptySlot, Slot newSlot)
        {
            var emptySlotBefore = new EmptySlot(currentEmptySlot.BeginTime, newSlot.BeginTime.AddMinutes(-1));
            var emptySlotAfter = new EmptySlot(newSlot.EndTime.AddMinutes(1), currentEmptySlot.EndTime);

            var result = new List<SlotBase> { emptySlotBefore, newSlot, emptySlotAfter };
            return result;
        }

        private ISlot GetSlotByTime(DateTime time)
        {
            var slot = slots.FirstOrDefault(s => time.IsBetween(s.BeginTime, s.EndTime));
            return slot;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns>empty slot or null if the slot in this time isn't empty</returns>
        private EmptySlot GetEmptySlotByTime(DateTime time)
        {
            var result = GetSlotByTime(time) as EmptySlot;
            return result;
        }


    }
}
