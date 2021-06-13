using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Quze.DAL;
using Quze.DAL.Stores;

namespace BLTests
{
    class FillServiceQueuesTest
    {
        [Test]
        public void fillServiceQueuesTest()
        {
            var ctx = new QueueStore().ctx;
            var timeTables = new GetEntitiesTemporary(ctx).GetTimeTablesWithIncludes().ToList();
            var timeTableLogic = new TimeTableLogic(timeTables , ctx.TimeTableExceptions, ctx.TimeTableVacations,
                1000000, ctx.ServiceQueues);
         var result =   timeTableLogic.CreateSQsInRangeOfDatesByTTs(DateTime.Now.Date).ToList();
         ctx.ServiceQueues.AddRange(result);
         ctx.SaveChanges();
        }
    }
}
