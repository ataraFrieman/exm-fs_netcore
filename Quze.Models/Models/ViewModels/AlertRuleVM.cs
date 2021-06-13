using Quze.Models.Models.Alerts;

namespace Quze.Models.Models.ViewModels
{
    public class AlertRuleVM: BaseVM
    {
        public AlertType AlertTypeID { get; set; }
        public int? RequiredTaskID { get; set; }
        public int? RequiredDocumentID { get; set; }
        public string Description { get; set; }
        public int HoursBeforeToAlert { get; set; }
        public int? RepeatInterval { get; set; }
    }
}
