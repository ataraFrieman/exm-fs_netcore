using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class ServiceTypeOppVM: BaseVM
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ParentServiceId { get; set; }
        public int? OrganizationId { get; set; }
        public int? Cost { get; set; }
        public List<RequiredTask> RequiredTasks { get; set; }
        public List<RequiredDocument> RequiredDocuments { get; set; }
        public List<RequiredTest> RequiredTests { get; set; }
        public List<ServiceProvidersServiceType> ServiceProvidersServiceTypes { get; set; }

        public virtual List<MinimalKitRules> MinimalKitRules { get; set; }
        public bool? IsVisibleToApp { get; set; }
        public bool? IsVisibleToOrganization { get; set; }
    }
}
