using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Models.ViewModels
{
    [Table("MinimalKitRules")]
    public class MinimalKitRulesVM : BaseVM
    {

        public ServiceTypeVM ServiceType { get; set; }
        public int ServiceTypeId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
