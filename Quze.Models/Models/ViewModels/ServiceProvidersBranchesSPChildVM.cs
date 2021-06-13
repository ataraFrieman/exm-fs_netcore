namespace Quze.Models.Models.ViewModels
{
    public class ServiceProvidersBranchesSPChildVM : BaseVM
    {
        public int ServiceProviderId { get; set; }
        public int BranchId { get; set; }
        public BranchVM Branch { get; set; }
    }
}