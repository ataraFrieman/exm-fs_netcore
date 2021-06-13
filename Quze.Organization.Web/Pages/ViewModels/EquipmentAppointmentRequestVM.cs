using Quze.Organization.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.Pages.ViewModels
{
    public class EquipmentAppointmentRequestVM: BaseVM
    {
        public int? EqpId { get; set; }
        public int? OperationId { get; set; }
        public int? Supplied { get; set; }
        public EquipmentVM Equipment { get; set; }
    }
}
