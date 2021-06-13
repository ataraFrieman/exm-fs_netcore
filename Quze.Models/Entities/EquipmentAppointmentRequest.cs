using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Entities
{
    public class EquipmentAppointmentRequest: EntityBase
    {
        public int EqpId { get; set; }
        public int? OperationId { get; set; }
        public bool? Supplied { get; set; }
        public Equipment Equipment { get; set; }
        public Operation Operation { get; set; }
    }
}
