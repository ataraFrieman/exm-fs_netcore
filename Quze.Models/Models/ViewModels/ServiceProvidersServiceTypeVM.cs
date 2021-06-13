namespace Quze.Models.Models.ViewModels
{
    public class ServiceProvidersServiceTypeVM:BaseVM
    {
        public int ServiceProviderId { get; set; }
        public ServiceProviderVM ServiceProvider { get; set; }
        public int ServiceTypeId { get; set; }
        public int AvgDuration { get; set; }
        public ServiceTypeVM ServiceType { get; set; }
      

    }
}
