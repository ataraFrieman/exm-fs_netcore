
using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Quze.DAL.Stores
{
    public class RequiredTaskStore : StoreBase<RequiredTask>
    {

        public RequiredTaskStore(QuzeContext ctx):base(ctx)
        {
                
        }

        public List<RequiredTask> GetTasksByST(int id)
        {
            var tasks = ctx.RequiredTasks
               //.Include(doc => doc.AlertRules)
               .Where(task => task.ServiceTypeID == id)
               .OrderBy(task => task.TimeUpdated)
               .Select(task => task
              ).ToList();

            return tasks;
        }

    }
}
