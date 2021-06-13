using System.ComponentModel.DataAnnotations.Schema;
namespace Quze.Models.Entities
{
    [Table("Fellows")]
    public class Fellow : Person
    {
        public int OrganizationId { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public bool? diabetis { get; set; }
        public bool? hypertension { get; set; }



        public User ApplicationUser { get; set; }
    }


}
