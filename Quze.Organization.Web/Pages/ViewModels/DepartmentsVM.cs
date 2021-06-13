using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class DepartmentsVM : BaseVM
    {
        public string Code { get; set; }
        public int DepartmentsTypesId { get; set; }

        public string Description { get; set; }
    }
}
