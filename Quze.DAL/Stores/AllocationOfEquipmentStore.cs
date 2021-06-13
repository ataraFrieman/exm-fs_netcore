using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quze.DAL.Stores
{
    public class AllocationOfEquipmentStore: StoreBase<AllocationOfEquipment>
    {
        EquipmentStore equipmentStore;
        public AllocationOfEquipmentStore(QuzeContext ctx):base(ctx)
        {
            equipmentStore = new EquipmentStore(ctx);
        }

      
      

        public AllocationOfEquipment GetAllocationOfEquipmentByAppointmentId(int appId,int eqpId)
        {
            var allocationOfEquipmenAppointmentQuery = ctx.AllocationOfEquipment.Where(eq => eq.AppointmentId == appId&& eq.EqpId== eqpId);
            AllocationOfEquipment appointmentTbl = allocationOfEquipmenAppointmentQuery.FirstOrDefault();
            return appointmentTbl;
        }

        public void AddallocationOfEquipmenToAppointment(int eqpId, Appointment appointment)
        {
            AllocationOfEquipment allocationOfEquipment = new AllocationOfEquipment();
            allocationOfEquipment.EqpId = eqpId;
            allocationOfEquipment.AppointmentId = appointment.Id;
            allocationOfEquipment.Amount++;
            allocationOfEquipment.BeginTime = appointment.BeginTime;
            allocationOfEquipment.EndTime = appointment.EndTime;
            ctx.AddAsync(allocationOfEquipment);
            ctx.SaveChangesAsync();
        }
        //remove equipment taken
        public void RemoveAllocationOfEquipment(int eqpId, AllocationOfEquipment allocationOfEquipment)
        {
            ctx.AllocationOfEquipment.Remove(allocationOfEquipment);
            equipmentStore.UpdateEquipmentEnabled(-1, eqpId);//update equipment
        }

        public AllocationOfEquipment AddAllocationOfEquipment(int eqpId, Appointment appointment)
        {
            
            Equipment isEnabled = equipmentStore.CheckIfEquipmentIsPossible(1, eqpId);
            if(isEnabled.IsNotNull())//enabled 
            {
                AllocationOfEquipment allocationOfEquipment = new AllocationOfEquipment()
                {
                    EqpId = eqpId,
                    AppointmentId = appointment.Id,
                    Amount = 1,
                    BeginTime = appointment.BeginTime,
                    EndTime = appointment.EndTime,
                    Equipment= isEnabled
                };
                var eqp = ctx.Add(allocationOfEquipment);
                ctx.SaveChanges();
                return allocationOfEquipment;
            }
            return null;//disabled
        }
    }
}
