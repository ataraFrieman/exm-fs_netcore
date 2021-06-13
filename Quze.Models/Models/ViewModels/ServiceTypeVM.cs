using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Models.ViewModels
{
    [Table("servicetypes")]
    public class ServiceTypeVM : BaseVM
    {
        public int OrganizationId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ParentServiceId { get; set; }
        public int? Cost { get; set; }
    }
}
