using System;
using System.Collections.Generic;

namespace Quze.Models.Entities
{
    public class RequiredTask: EntityBase
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public int? ServiceTypeID { get; set; }
        //public ServiceType ServiceType { get; set; }

        //public List<AlertRule> AlertRules  { get; set; }
    }
}
