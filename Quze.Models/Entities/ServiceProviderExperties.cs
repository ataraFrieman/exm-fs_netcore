using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("ServiceProvidersExperties")]
    public class ServiceProviderExperties : EntityBase
    {
        public int ServiceProviderId { get; set; }
        public int ExpertyId { get; set; }

    }
}
