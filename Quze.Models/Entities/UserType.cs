using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("UserTypes")]
    public class UserType 
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }


        public const int ApplicationUser = 1;
        public const int OrganizationUser = 2;
        public const int SuperUser = 3;
    }
}
