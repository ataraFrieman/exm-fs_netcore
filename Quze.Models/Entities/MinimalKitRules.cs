using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("MinimalKitRules")]
    public class MinimalKitRules : EntityBase
    {
        public ServiceType ServiceType { get; set; }
        public int ServiceTypeId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
