namespace Quze.Models.Entities
{
    public class ServiceProvidersServiceType:EntityBase
    {
        public int ServiceProviderId { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public int ServiceTypeId { get; set; }
        public int AvgDuration { get; set; }
        public ServiceType ServiceType { get; set; }
    }
}
