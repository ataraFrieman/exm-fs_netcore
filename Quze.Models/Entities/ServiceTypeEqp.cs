using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quze.Models.Entities
{
    public class ServiceTypeEqp:EntityBase
    {
        public int? EqpId { get; set; }
        public int? ServiceTypeId { get; set; }
        //[NotMapped]
        //public Equipment Equipment { get; set; } = null;
    }
}

