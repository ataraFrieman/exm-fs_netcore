namespace Quze.Models.Entities
{
    public class ServiceProvidersBranches : EntityBase
    {
        public int ServiceProviderId { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        //public virtual List<MinimalKitRules> MinimalKitRules { get; set; }

    }
}
