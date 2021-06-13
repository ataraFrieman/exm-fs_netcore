using System;
using System.Collections.Generic;

namespace Quze.Models.Entities
{
    public class RequiredDocument: EntityBase
    {
        public String Code { get; set; }
        public String Description { get; set; }
        public bool IsRequired { get; set; }
        public int? ServiceTypeID { get; set; }
        //public int HoursBeforeToAlert { get; set; }
        //public ServiceType ServiceType { get; set; }

        //public List<AlertRule> AlertRules { get; set; }
    }
}
