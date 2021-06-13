using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quze.Models.Entities
{
    public class Equipment : EntityBase
    {
        public string EqpType { get; set; } = null;
        public string EqpDescription { get; set; } = null;
        public int OrganizationId { get; set; }
        public int? MaximumAmount { get; set; }
        public bool? IsQuantityLeft { get; set; }
        public int? NumAllow { get; set; }



        //[NotMapped]
        //public List<ServiceTypeEqp> Equipments { get; set; } = null;
        [NotMapped]
        public List<AllocationOfEquipment> AllocationOfEquipment { get; set; } = null;
        public List<EquipmentAppointmentRequest> EquipmentAppointmentRequest { get; set; } = null;

        

    }
}
