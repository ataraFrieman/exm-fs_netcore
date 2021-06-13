using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("Streets")]
   public class Street: EntityBase
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public int StreetSymbol { get; set; }
        public virtual City City { get; set; }
    }
}
