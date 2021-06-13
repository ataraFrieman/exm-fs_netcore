using System.Collections.Generic;

namespace Quze.Organization.Web.ViewModels
{
    public class ServiceProviderVM : BaseVM
    {
        public int ServiceTypeId { get; set; }
        public int OrganizationId { get; set; }
        public ServiceTypeVM ServiceType { get; set; }
        public OrganizationVM Organization { get; set; }
        public string FullName { get; set; }
        //public virtual List<ServiceProvidersServiceTypeVM> ServiceProvidersServiceTypes { get; set; }

    }
}
