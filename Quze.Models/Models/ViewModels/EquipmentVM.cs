using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Models.ViewModels
{
    public class EquipmentVM : BaseVM
    {
        public int organizationId { get; set; }
        public string EqpType { get; set; } = null;
        public string EqpDescription { get; set; } = null;
        public int? MaximumAmount { get; set; }
        public bool? IsQuantityLeft { get; set; }

        public List<AllocationOfEquipment> AllocationOfEquipment { get; set; } = null;

    }
}
