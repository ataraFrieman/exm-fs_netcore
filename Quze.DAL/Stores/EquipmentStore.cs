using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quze.DAL.Stores
{
    public class EquipmentStore: StoreBase<Equipment>
    {
        public EquipmentStore(QuzeContext ctx):base(ctx)
        {

        }

        public List<Equipment> GetEquipmentsByOrganization(int organizationId)
        {
            List<Equipment> equipmentList = new List<Equipment>();
            var equipmentListQuery = ctx.Equipment.Where(equipment => equipment.OrganizationId == organizationId);
            equipmentList= equipmentListQuery.ToList();
            return equipmentList;
        }

        //updat equipment
        public void UpdateEquipmentEnabled(int numEqp,int EqpId)
        {
            var equipmentQuery = ctx.Equipment.Where(eq => eq.Id == EqpId);
            Equipment equipment= equipmentQuery.FirstOrDefault();
            equipment.NumAllow += numEqp;
            if (equipment.IsQuantityLeft==false)//disabled
                equipment.IsQuantityLeft = true;//enabled
            ctx.SaveChanges();
        }

        internal Equipment CheckIfEquipmentIsPossible(int numEqp,int eqpId)
        {
            var equipmentQuery = ctx.Equipment.Where(eq => eq.Id == eqpId);
            Equipment equipment=equipmentQuery.FirstOrDefault();
            var numAllow = equipment.NumAllow + numEqp;
            if(equipment.IsQuantityLeft== false|| equipment.MaximumAmount<numAllow)
                return null;
            if(equipment.MaximumAmount== numAllow)
                equipment.IsQuantityLeft = false;
            equipment.NumAllow += numEqp;
            ctx.SaveChanges();
            return equipment;
        }
    }
}
