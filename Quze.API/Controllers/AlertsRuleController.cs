using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class AlertsRuleController : ControllerBase<AlertRule, AlertRuleVM,AlertRuleStore>
    {
        public AlertsRuleController(IMapper mapper, AlertRuleStore store) : base(mapper, store)
        {

        }
    }
}