using Quze.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.DAL.Stores
{
    public class AlertRuleStore : StoreBase<AlertRule>
    {

        public AlertRuleStore(QuzeContext ctx) : base(ctx)
        {


        }

        public  List<AlertRule> GetAlertByTaskId(int taskId)
        {
            var list = ctx.AlertRule
                .Where(alert => alert.RequiredTaskID == taskId)
                .ToList();
            return list;
        }

    }
}
