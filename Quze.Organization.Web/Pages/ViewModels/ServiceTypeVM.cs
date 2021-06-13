using Quze.Models.Entities;
using System.Collections.Generic;

namespace Quze.Organization.Web.ViewModels
{
    public class ServiceTypeVM : BaseVM
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ParentServiceId { get; set; }
        public int SpId { get; set; }
        public List<ServiceProvider> SP { get; set; }
        public List<ServiceQueue> Queue { get; set; }


    }
}
