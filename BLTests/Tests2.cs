using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Quze.Models.Entities;
using Quze.BL.UserQueue.UserConstraint;
using Quze.DAL;
using Quze.BL.QueuingManagement;
using Quze.DAL.Stores;
using Quze.Models;
using System.Threading;
using System.Diagnostics;
using Quze.Infrastruture.Types;
using Quze.Infrastruture.Utilities;

namespace Tests
{
    public class Tests2
    {
        private TimeTableStore timeTableStore;
        private TimeTableExceptionStore ttExceptionStore;
        private TimeTableVacationStore ttVactationStore;
        private ServiceQueueStore sqStore;
        private OrganizationStore organizationStore;
        private ServiceProviderStore serviceProviderStore;
        private BranchStore branchStore;


        private Organization organizationCriteria;
        private List<ServiceProvider> serviceProvidersCriteria;

        private UserConstraints userConstraints;
        private PossibleTime possibleTime;
        private QuzeContext context;
        public Tests2(QuzeContext ctx)
        {
            context = ctx;
        }

        // [SetUp]
        //public void Setup()
        //{
        //    timeTableStore = new TimeTableStore();
        //    ttExceptionStore = new TimeTableExceptionStore();
        //    ttVactationStore = new TimeTableVacationStore();
        //    sqStore = new ServiceQueueStore();
        //    organizationStore = new OrganizationStore();
        //    serviceProviderStore = new ServiceProviderStore();
        //    branchStore = new BranchStore();

        //    possibleTime = new PossibleTime(DateTime.Now, DateTime.Now.AddDays(1));

        //  //  organizationCriteria = organizationStore.ToListAsync().Result.FirstOrDefault(o => o.Id == 5);
        //    serviceProvidersCriteria = serviceProviderStore.ToListAsync().Result.Where(w => w.Id == 5).ToList();
        //    userConstraints = new UserConstraints(organizationCriteria,null,serviceProvidersCriteria,null, new DatesPossible(new List<PossibleTime>{possibleTime}));
        //}

        [Test]
        public void ReadAppSettings()
        {
           var url= AppConfiguration.AppSettings("ML_URL");
        }

        [Test]
        public void CTXMultyTrhededTest()
        {
            


            var t1 = Task.Run(() => { Debug.WriteLine( "-----------------"+Thread.CurrentThread.ManagedThreadId); return context.TimeTables.ToListAsync(); });
            var t2 = Task.Run(() => { Debug.WriteLine( "-----------------"+Thread.CurrentThread.ManagedThreadId); return context.Branches.ToListAsync();        });
            var t3 = Task.Run(() => { Debug.WriteLine("-----------------"+Thread.CurrentThread.ManagedThreadId);return context.Organizations.ToListAsync();    });
            var t4 = Task.Run(() => { Debug.WriteLine("-----------------"+Thread.CurrentThread.ManagedThreadId);return context.ServiceProviders.ToListAsync(); });



            Task.WaitAll(t1, t2, t3, t4);
            var r1 = t1.Result;
            var r2 = t2.Result;

        }

        [Test]
        public void IntegraionTest()
        {
            //   Setup();

            // var userConstraint2 = userConstraints;

            //  var q3 = new QueueManager3(userConstraints, timeTableStore.ToListAsync().Result, ttExceptionStore.ToListAsync().Result, ttVactationStore.ToListAsync().Result, sqStore.ToListAsync().Result);

            //  q3.Run();
        }

        [Test]
        public void StTest()
        {
            //    var queueStore = new QueueStore();
            //   queueStore.CreateTimeTableLine(new TimeTable());

            var serviceTypeStore = new ServiceTypeStore(context);
            var a = serviceTypeStore.GetDescendants(2).ToList();
            var result = serviceTypeStore.GetDescendants(1).ToList();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var before = watch.Elapsed;
            var result2 = serviceTypeStore.GetDescendants(12).ToList();
            var after = watch.Elapsed;
        }

        [Test]
        public void StTest2()
        {
            var serviceTypeStore = new ServiceTypeStore(context);
            IEnumerable<int> stIds = new List<int> { 12, 3 };
            var timeTablesQuery = serviceTypeStore.ctx.TimeTables;
            var a = from timeTable in timeTablesQuery
                    from stId in stIds
                    where stId == timeTable.ServiceTypeId
                    select timeTable;
        }

        [Test]
        public void StTest3()
        {
            //var serviceTypeStore = new ServiceTypeStore();
            //var queueStore = new QueueStore();
            ////   var timeTablesQuery = serviceTypeStore.ctx.TimeTables;
            //var request = new GetAvailableSlotsRequest {ServiceTypeId = 3};

            //var result =  queueStore.FilterTimeTablesByOrganization(request.OrganizationId?? default(int), queueStore.ctx.TimeTables);
        }

        [Test]
        public void addTurnToColonsopia()
        {


            var queueStore = new QueueStore(context);

            var request = new GetAvailableSlotsRequest
            {
                ArrivalTime = 500,
                BeginTime = DateTime.Today,
                CityId = 3650,
                EndTime = DateTime.Now.AddDays(20),
                ServiceTypeId = 216,
            };

            var respone = queueStore.GetAvailableSlotsBySt(request);
        }

        [Test]
        public void createTimeTableLines()
        {
            string uri = "TimeTableLines";
            var queueStore = new QueueStore(context);
            var ctx = queueStore.ctx;
            var timeTableDraw = ctx.TimeTables.ToList();
            var timeTableLineDraw = ctx.TimeTableLines.ToList();
            Random random = new Random(DateTime.Now.Millisecond);
           
            foreach (var item in timeTableDraw)
            {
                if (timeTableLineDraw.Any(ctc => ctc.TimeTableId == item.Id))
                    continue;

                var daysOfWork = random.Next(1, 5);



                for (int n = 0; n <= daysOfWork; n++)
                {
                    var findDay = random.Next(1, 5);
                    QuzeDayOfWeek d = (QuzeDayOfWeek)findDay;
                    var beginHours = random.Next(8, 12);
                    var minutes = random.Next(0, 1) == 1 ? 30 : 0;
                    var secounds = 0;
                    var endHours = random.Next(16, 21);
                    TimeSpan time1 = new TimeSpan(beginHours, minutes, secounds);
                    TimeSpan time2 = new TimeSpan(endHours, minutes, secounds);
                    TimeTableLine ttl = new TimeTableLine()
                    {
                        TimeTableId = item.Id,
                        WeekDay = d,
                        BeginTime = time1,
                        EndTime = time2
                    };
                    var temp = ttl;
                    if (item.TimeTableLines.Any(ckc => ckc.WeekDay == temp.WeekDay))
                        break;



                    item.TimeTableLines.Add(ttl);
                }


            }
            //var chacks = true;
            //foreach(var TTD in timeTableDraw)
            //{
            //    if (timeTableDraw.Any(ckc => ckc.TimeTableLines.IsNullOrEmpty()))
            //        chacks = false;
            //}
            //Assert.IsTrue(chacks);
            var allTimeTableLines = new List<TimeTableLine>();
            foreach (var tt in timeTableDraw)
            {
                allTimeTableLines.AddRange(tt.TimeTableLines);
            }
            ctx.AddRange(allTimeTableLines);
            ctx.SaveChanges();


        }
    }





    }











