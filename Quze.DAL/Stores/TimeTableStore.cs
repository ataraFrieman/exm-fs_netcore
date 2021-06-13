using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;

namespace Quze.DAL.Stores
{
    public class TimeTableStore : StoreBase<TimeTable>
    {

        public TimeTableStore(QuzeContext ctx):base(ctx)
        {
                
        }


        public override Task<List<TimeTable>> ToListAsync()
        {

          var timeTables = ctx.TimeTables
              .Include( tt => tt.TimeTableLines)
              .ToListAsync();
          return timeTables;
        }
    }
}
