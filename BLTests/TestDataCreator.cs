using AutoMapper;
using NUnit.Framework;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLTests
{

    public class Source
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }
    }

    public class Destination
    {
        public int Total { get; set; }
    }




    public class TestDataCreator
    {

        [Test]
        public void MapperTest()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Source, Destination>(MemberList.None));

            Mapper.AssertConfigurationIsValid();

        }

        public TimeTable CreateTimeTable(int spId, int branchId, DateTime? validDate = null, DateTime? untilDate = null)
        {
            var tt = new TimeTable();
            tt.ServiceProviderId = spId;
            tt.BranchId = branchId;
            tt.ValidFromDate = validDate != null ? validDate.Value : DateTime.Today.AddDays(-5);
            tt.ValidUntilDate = untilDate != null ? untilDate.Value : DateTime.Today.AddDays(200);
            var rnd = new Random(DateTime.Now.Millisecond);
            var ttlCount = rnd.Next(1, 10);
            for (int i = 0; i <= ttlCount; i++)
            {
                var weekDay = rnd.Next(1, 7);
                var beginHour = rnd.Next(7, 17);
                var beginTime = new TimeSpan(beginHour, 0, 0);
                // var ttl = new TimeTableLine((QuzeWeekDay)weekDay, beginTime, beginTime.Add());

            }
            return tt;
        }



    }
}
