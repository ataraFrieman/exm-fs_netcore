using System.Collections.Generic;
using Quze.Models.Models.ViewModels;

namespace Quze.API.ViewModels
{
    public class ServiceProvidersExpertiesVM : BaseVM
    {
        public int ServiceProviderId { get; set; }
        public int ExpertyId { get; set; }
    }
}
