using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quze.Infrastruture.Extensions;
using Quze.Models;

namespace Quze.Organization.Web.Controllers
{
    public class AlertRulesController : ControllerBase<AlertRule, AlertRuleVM, AlertRuleStore>
    {
        public AlertRulesController(IMapper mapper, AlertRuleStore store) : base(mapper, store)
        {

        }

        [HttpGet("[action]")]
        public Quze.Models.Response<AlertRuleVM> GetAlertByTaskId(int taskId)
        {
            var entities = store.GetAlertByTaskId(taskId);
            var entitiesVM = mapper.Map<List<AlertRuleVM>>(entities);
            var response = new Quze.Models.Response<AlertRuleVM> { Entities = entitiesVM };
            return response;
        }
    }


}