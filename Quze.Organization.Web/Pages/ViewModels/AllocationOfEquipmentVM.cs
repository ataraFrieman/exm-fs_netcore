using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class AllocationOfEquipmentVM:BaseVM
    {
        public int? EqpId { get; set; }
        public int? AppointmentId { get; set; }
        public int? Amount { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public EquipmentVM Equipment { get; set; } = null;
    }
}
