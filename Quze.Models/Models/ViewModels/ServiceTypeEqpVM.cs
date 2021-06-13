using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Models.Models.ViewModels
{
    public class ServiceTypeEqpVM : BaseVM
    {
        public int? EqpId { get; set; }
        public int? ServiceTypeId { get; set; }

        public List<EquipmentVM> Equipments { get; set; } = null;
    }
}
