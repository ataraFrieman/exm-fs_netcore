using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class ServiceTypeEqpVM: BaseVM
    {
        public int? EqpId { get; set; }
        public int? ServiceTypeId { get; set; }
        
        public EquipmentVM Equipment { get; set; } = null;
    }
}
