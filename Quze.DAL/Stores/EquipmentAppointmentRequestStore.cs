using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quze.DAL.Stores
{
   public class EquipmentAppointmentRequestStore: StoreBase<EquipmentAppointmentRequest>
    {
        public EquipmentAppointmentRequestStore(QuzeContext ctx):base(ctx)
        {

        }

        

        public List<EquipmentAppointmentRequest> GetEquipmentsAppointments(int operationId)
        {
            return ctx.EquipmentAppointmentRequest.Where(eq => eq.OperationId == operationId).ToList();
        }

        public void RemoveEquipmentAppointment(EquipmentAppointmentRequest equipment)
        {
            ctx.EquipmentAppointmentRequest.Remove(equipment);
            ctx.SaveChanges();
        }

        public EquipmentAppointmentRequest CreateEquipmentAppointmentRequest(int eqpId, int Oppid)
        {
            EquipmentAppointmentRequest equipmentAppointmentRequest = new EquipmentAppointmentRequest()
            {
                EqpId = eqpId,
                OperationId = Oppid,
                Supplied = false
            };
            return equipmentAppointmentRequest;
        }

        public void AddEquipmentAppointment(List<EquipmentAppointmentRequest> addEquipmentsAppointments)
        {
            ctx.EquipmentAppointmentRequest.AddRange(addEquipmentsAppointments);
            ctx.SaveChanges();
        }
    }
}
