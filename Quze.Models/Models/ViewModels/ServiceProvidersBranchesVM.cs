namespace Quze.Models.Models.ViewModels
{
    public class ServiceProvidersBranchesVM : BaseVM
    {
        public int ServiceProviderId { get; set; }
        public ServiceProviderVM ServiceProvider { get; set; }
        public int BranchId { get; set; }
        public BranchVM Branch { get; set; }
    }
}
