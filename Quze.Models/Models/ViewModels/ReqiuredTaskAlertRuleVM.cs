using Quze.Models.Models.Alerts;
using System.Collections.Generic;

namespace Quze.Models.Models.ViewModels
{
    public class ReqiuredTaskAlertRuleVM : BaseVM
    {
        public List<AlertRuleVM> AlertsRules { get; set; }
        public RequiredTaskVM Task { get; set; }

    }
}
