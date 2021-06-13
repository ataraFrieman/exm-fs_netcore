using System.Collections.Generic;
using Quze.Models.Models.ViewModels;

namespace Quze.API.ViewModels
{
    public class ServiceTypeVM : BaseVM
    {
        public int OrganizationId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ParentServiceId { get; set; }
        public int Cost { get; set; }
        public List<RequiredTasksVM> RequiredTasks { get; set; }
        public List<RequiredDocumentVM> RequiredDocuments { get; set; }
        public bool? IsVisibleToApp { get; set; }
        public bool? IsVisibleToOrganization { get; set; }
    }
}