using System.Collections.Generic;
using Quze.Models;
using Quze.Models.Models;

namespace Quze.API.ViewModels
{
    public class SP_BranchSlots
    {
        public int ServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public ServiceTypeVM ServiceType { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public List<Slot> Slots { get; set; }
    }
}