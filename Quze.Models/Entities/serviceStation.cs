using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("ServiceStations")]
    public class ServiceStation:EntityBase
    {
        public string Description { get; set; }
        public int? StationsNumber { get; set; }
        public int? BranchId { get; set; }
        public int? OrganizaionId { get; set; }
        public string Location { get; set; } = null;
    }
}
