using System.Collections.Generic;

namespace MohServiceType
{
    public class ServiceTypeVM 
    {
        public int? Id { get; set; }
        public int OrganizationId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ParentServiceId { get; set; }
        public int? Cost { get; set; }
        public bool? IsVisibleToApp { get; set; }
        public bool? IsVisibleToOrganization { get; set; }
    }
}