using System;
using System.Collections.Generic;

namespace Quze.Models.Models.ViewModels
{
    public class RequiredTaskVM: BaseVM
    {
        public String Code { get; set; }
        public String Description { get; set; }
        public bool IsRequired { get; set; }
        public int? ServiceTypeID { get; set; }

        //public int ServiceTypeID { get; set; }
        //public ServiceTypeVM ServiceType { get; set; }
        //public List<AlertRuleVM> AlertRules { get; set; }

    }
}
