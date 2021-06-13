using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Models.ViewModels
{
    [Table("Fellows")]
    public class FellowVM : PersonVM
    {
        public int OrganizationId { get; set; }
        public string PhoneNumber { get; set; }

        public UserVM ApplicationUser { get; set; }
    }


}
