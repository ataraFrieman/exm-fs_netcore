using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class EquipmentVM:BaseVM
    {
        public int organizationId { get; set; }
        public string EqpType { get; set; } = null;
        public string EqpDescription { get; set; } = null;
        public int? MaximumAmount { get; set; }
        public bool? IsQuantityLeft { get; set; }
        public int? NumAllow { get; set; }
    }
}
