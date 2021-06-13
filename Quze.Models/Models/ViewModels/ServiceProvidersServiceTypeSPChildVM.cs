namespace Quze.Models.Models.ViewModels
{
    public class ServiceProvidersServiceTypeSPChildVM : BaseVM
    {
        public int ServiceProviderId { get; set; }
        public int ServiceTypeId { get; set; }
        public int AvgDuration { get; set; }
        public ServiceTypeVM ServiceType { get; set; }


    }
}
