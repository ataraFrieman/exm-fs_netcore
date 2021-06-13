using Quze.Models.Models.Alerts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quze.Models.Entities
{
    [Table("AlertRules")]
    public class AlertRule : EntityBase
    {
        public AlertType AlertTypeID { get; set; }
        public int ? RequiredTaskID { get; set; }
        public int ? RequiredDocumentID { get; set; }
        public string Description { get; set; }
        public int HoursBeforeToAlert { get; set; }
        public int? RepeatInterval { get; set; }
    }
}
