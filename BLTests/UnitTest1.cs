using BLTests;
using NUnit.Framework;
using Quze.BL.Utiles;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Types;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tests
{
    public class Tests
    {
        private QuzeContext ctx;
        /*
                List<ServiceProvider> serviceProvider = new List<ServiceProvider>();
                List<ServiceProvidersServiceType> serviceProvidersServiceTypes = new List<ServiceProvidersServiceType>();
                List<Organization> organizations = new List<Organization>();
                List<Branch> branches = new List<Branch>();
                private QuzeDayOfWeek Tuesday;

                // ServiceType service = new ServiceType();
                //List<ServiceProvidersServiceType> serviceProvidersServiceTypes = new List<ServiceProvidersServiceType>();
                [SetUp]
                public void Setup()
                {

                }

                [Test]

                public void GetRelaventServieceProvidersTest()
                {

                    //List<ServiceProvider> serviceProvider = new List<ServiceProvider>();

                    //List<ServiceProvider> serviceProvider = new List<ServiceProvider>();
                    //List<ServiceProvidersServiceType> serviceProvidersServiceTypes = new List<ServiceProvidersServiceType>();
                    // List<Organization> organizations = new List<Organization>();
                    //List<Branch> branches = new List<Branch>();
                    // ServiceType service = new ServiceType();
                    //List<ServiceProvidersServiceType> serviceProvidersServiceTypes = new List<ServiceProvidersServiceType>();

                    CreateAndAddSP(9, 9);
                    CreateAndAddSP(3, 3);
                    CreateAndAddSP(4, 4);
                    CreateAndAddSP(11, 11);
                    CreateAndAddSP(9, 9);

                    createAndAddSPST(9, 11);
                    createAndAddSPST(3, 5);
                    createAndAddSPST(4, 6);
                    createAndAddSPST(11, 13);
                    createAndAddSPST(9, 11);



                    Organization organization = new Organization()
                    {
                        Id = 9,
                    };
                    ServiceType serviceType = new ServiceType()
                    {
                        OrganizationId = 9,
                    };

                    ServiceType service = new ServiceType()
                    {
                        Id = 11,

                    };
                    Branch branch = new Branch();
                    //List<Branch> branches = new List<Branch>() { branch };
                    ConstraintsBase constraintsBase = new ConstraintsBase()
                    {
                        Organization = organization,
                        ServiceType = service,
                    };
                    SearchResultType searchResultType1 = new SearchResultType();
                    List<SearchResultType> searchResultTypes = new List<SearchResultType>() { searchResultType1 };
                    ServiceQueue serviceQueue1 = new ServiceQueue();
                    List<ServiceQueue> serviceQueues = new List<ServiceQueue>() { serviceQueue1 };
                    TimeTable timeTable1 = new TimeTable();
                    List<TimeTable> timeTables = new List<TimeTable>() { timeTable1 };
                    TimeTableVacation timeTableVacation1 = new TimeTableVacation();
                    List<TimeTableVacation> timeTableVacations = new List<TimeTableVacation>() { timeTableVacation1 };





                //    QueueManager Qtest = new QueueManager(constraintsBase, serviceProvider, serviceProvidersServiceTypes, serviceQueues, timeTables, timeTableVacations);





                    var sps = Qtest.FilterServiceProvidersByConstraint(serviceProvider);
                    int count = 0;
                    foreach (ServiceProvider element in sps)
                    {
                        count++;
                    }
                    int expected = 2;
                    Assert.IsTrue(count == expected);


                }

                private ServiceProvider CreateSP(int id, int oid)
                {

                    return new ServiceProvider()
                    {
                        Id = id,
                        OrganizationId = oid,

                    };
                }


                private void CreateAndAddSP(int id, int oid)
                {
                    this.serviceProvider
                        .Add(CreateSP(id, oid));
                }


                private ServiceProvidersServiceType CreateSPST(int id, int serviceId)
                {
                    return new ServiceProvidersServiceType()
                    {
                        Id = id,
                        ServiceTypeId = serviceId,
                    };
                }
                private void createAndAddSPST(int id, int serviceId)
                {
                    this.serviceProvidersServiceTypes.Add(CreateSPST(id, serviceId));
                }


                private ConstraintsBase CreateCONSB(Organization organization, ServiceType service, List<ServiceProvider> serviceProvider, List<Branch> branches)
                {
                    return new ConstraintsBase()
                    {
                        Organization = organization,
                        ServiceType = service,
                        ServiceProvider = serviceProvider,
                        Branch = branches,
                    };
                }


                private Organization CreatOrganization(int id)
                {
                    return new Organization()
                    {
                        OrganizationTypeCode = id,

                    };
                }
                private void CreateOrg(int id)
                {
                    this.organizations.Add(CreatOrganization(id));
                }

                private Branch createBranch(int id)
                {
                    return new Branch()
                    {
                        OrganizationId = id,
                    };
                }

                private void CreateBran(int id)
                {
                    this.branches.Add(createBranch(id));
                }

            */

        public Tests()
        {
            var queueStore = new QueueStore(ctx);
            ctx = queueStore.ctx;
        }

        [Test]
        public void Translate()
        {
            var t = new HebrewEnglishTranslator();
            var i = 1;
            foreach (var item in ctx.Streets.ToList())
            {

                item.Name = t.Translate(item.Name);
                Console.WriteLine("{0} - {1}", i, item.Name);
            }

            ctx.SaveChanges();
        }

        [Test]
        public void GetRelevantTimeTablesTest()



        {
            // quzeDayOfWeek = new QuzeDayOfWeek();
            QuzeDayOfWeek e = (QuzeDayOfWeek)3;
            QuzeDayOfWeek d = (QuzeDayOfWeek)4;
            TimeSpan time1 = new TimeSpan(9, 30, 0);
            TimeSpan time2 = new TimeSpan(4, 30, 0);
            TimeSpan time3 = new TimeSpan(10, 0, 0);
            TimeSpan time4 = new TimeSpan(4, 0, 0);
            TimeTable timeTable1 = new TimeTable
            {
                ServiceProviderId = 3,
                BranchId = 4,
                ServiceTypeId = 5,
                ValidFromDate = new DateTime(2000, 01, 01),
                ValidUntilDate = new DateTime(2020, 01, 01),
            };





            TimeTable timeTable2 = new TimeTable
            {
                ServiceProviderId = 4,
                BranchId = 6,
                ServiceTypeId = 2,
                ValidFromDate = new DateTime(2000, 03, 01),
                ValidUntilDate = new DateTime(2020, 03, 01),
            };



            //  var tt = timeTable1.[quzeDayOfWeek]; 
            timeTable1.AddTimeTableLine(e, time1, time2);
            timeTable1.AddTimeTableLine(d, time3, time4);
            var ttd = timeTable1.TimeTableLines;
            //Assert.IsNotEmpty(ttd);
            Assert.IsTrue(ttd.Count == 2);
            // List<TimeTableLine> timeTableLines = new List<TimeTableLine>();


            //    var sum = timeTable1.TimeTableLines;
            //timeTable1.TimeTableLines;


        }

        [Test]
        public void GetRelevantLine()
        {
            QuzeDayOfWeek e = (QuzeDayOfWeek)3;
            QuzeDayOfWeek d = (QuzeDayOfWeek)4;
            TimeSpan time1 = new TimeSpan(9, 30, 0);
            TimeSpan time2 = new TimeSpan(4, 30, 0);
            TimeSpan time3 = new TimeSpan(10, 0, 0);
            TimeSpan time4 = new TimeSpan(4, 0, 0);
            TimeTable timeTable1 = new TimeTable
            {
                ServiceProviderId = 3,
                BranchId = 4,
                ServiceTypeId = 5,
                ValidFromDate = new DateTime(2000, 01, 01),
                ValidUntilDate = new DateTime(2020, 01, 01),
            };


            //var t = ValidFromDate;


            TimeTable timeTable2 = new TimeTable
            {
                ServiceProviderId = 4,
                BranchId = 6,
                ServiceTypeId = 2,
                ValidFromDate = new DateTime(2000, 03, 01),
                ValidUntilDate = new DateTime(2020, 03, 01),
            };



            var tt = timeTable1[timeTable1.ValidFromDate];
            timeTable1.AddTimeTableLine(e, time1, time2);
            timeTable1.AddTimeTableLine(d, time3, time4);
            DateTime time5 = new DateTime(9);
            // var detect = timeTable1[ValidFromDate];
            // 
            //Assert.

        }

        [Test]
        public void TestTimeTableIndex()
        {
            //    var store = new TimeTableStore();
            //    var timeTables = store.ToListAsync().Result;
            //  var timeTable =  timeTables.Find(x => x.Id == 3);
            //    var date = new DateTime(2/5/2019);

            //    var result = timeTable[date];
            TimeTable TT = new TimeTable();
            QuzeDayOfWeek d = (QuzeDayOfWeek)4;
            TimeSpan time1 = new TimeSpan(9, 30, 0);
            TimeSpan time2 = new TimeSpan(4, 30, 0);
            TT.AddTimeTableLine(d, time1, time2);
            DateTime DT = new DateTime(2 / 5 / 2019);
            var TTL = TT[DT];
            Assert.IsNotNull(TTL);


        }

        [Test]
        public void AppointmentSetIsPossible()
        {

            QuzeDayOfWeek e = (QuzeDayOfWeek)3;
            QuzeDayOfWeek d = (QuzeDayOfWeek)4;
            TimeSpan time1 = new TimeSpan(9, 30, 0);
            TimeSpan time2 = new TimeSpan(4, 30, 0);
            TimeSpan time3 = new TimeSpan(10, 0, 0);
            TimeSpan time4 = new TimeSpan(4, 0, 0);
            TimeTable timeTable1 = new TimeTable
            {
                ServiceProviderId = 3,
                BranchId = 4,
                ServiceTypeId = 5,
                ValidFromDate = new DateTime(2000, 01, 01),
                ValidUntilDate = new DateTime(2020, 01, 01),
            };

            TimeTable timeTable2 = new TimeTable
            {
                ServiceProviderId = 4,
                BranchId = 6,
                ServiceTypeId = 2,
                ValidFromDate = new DateTime(2000, 03, 01),
                ValidUntilDate = new DateTime(2020, 03, 01),
            };


            timeTable1.AddTimeTableLine(e, time1, time2);
            timeTable1.AddTimeTableLine(d, time3, time4);
            var T = timeTable1.CanSetApoointmentToday(d);
            void CreateNameList()
            {
                var fileContent = File.ReadAllText(@"c:\Desktop\file.txt", Encoding.GetEncoding(1255)).Split(' ');
            }


            Assert.True(T);


        }

        [Test]

        public void CreateProvidersList()
        {
            //TODO : to convert the Arrays to Lists in order to removeAll the unwanted chars
            List<int> exsp = new List<int>();
            var firstNames = File.ReadAllText(@"c:\users\Barukh\Desktop\example.txt", Encoding.Default).Split(" ");
            firstNames = firstNames.Where(l => l.Trim().Length > 0).ToArray();

            var lastNames = File.ReadAllLines(@"c:\users\Barukh\Desktop\example2.txt", Encoding.Default);
            lastNames = lastNames.Where(l => l.Trim().Length > 0).ToArray();

            List<string> cites = new List<string> { "ירושלים", "תל אביב", "חיפה", "באר שבע", "צפת", "חולון", "רמת גן", " גבעתיים", "בת ים", "נתניה", "חדרה", "נהריה", "אילת" };


            List<ServiceProviderVM> list = new List<ServiceProviderVM>();
            string uri = "ServiceProviders";

            var queueStore = new QueueStore(ctx);
            ctx = queueStore.ctx;
            var orgId = ctx.Organizations.Select(x => x.Id).ToList();
            Random random = new Random();
            for (int i = 0; i <= lastNames.Length - 1; i++)
            {
                var firstNameIndex = new Random().Next(0, firstNames.Length - 1);
                var firstName = firstNames[firstNameIndex];
                var lastNameIndex = new Random().Next(0, lastNames.Length - 1);
                var lastname = lastNames[lastNameIndex];
                int current = new Random().Next(0, 100000000);
                string identityNumber = current.ToString();
                var orgIdIndex = new Random().Next(0, orgId.Count - 1);
                int organizationId = orgId[orgIdIndex];
                int current2 = random.Next(0, 1000);
                string licenseNumber = current2.ToString();
                int current3 = random.Next(0, 100);
                int cityId = current3;
                var find = new Random().Next(20, 80);
                var date = DateTime.Now;
                var licenseReceiptDate = date.AddYears(-find);


                var CityNameIndex = new Random().Next(0, cites.Count - 1);
                string cityName = cites[CityNameIndex];
                ServiceProviderVM SP = new ServiceProviderVM()
                {
                    IdentityNumber = identityNumber,
                    FirstName = firstName,
                    LastName = lastname,
                    OrganizationId = organizationId,
                    LicenseNumber = licenseNumber,
                    CityId = cityId,
                    LicenseReceiptDate = licenseReceiptDate
                };
                list.Add(SP);
            }

            HttpUtil.Post(uri, list);




        }
        [Test]

        public void CreateOrganizations()
        {
            List<OrganizationVM> organizationVMs = new List<OrganizationVM>();
            string uri = "Organizations";
            List<string> names = new List<string> { "שערי צדק ", "תל השומר", "שיבא", "בלינסון", "לאומית", "מכבי", "מאוחדת", "כללית" };

            for (int i = 0; i <= 7; i++)
            {
                var idt = new Random().Next(1035, 1045);
                int id = idt;
                int current = new Random().Next(0, 100);
                string mohCode = current.ToString();
                var oorganizationNameIndex = new Random().Next(0, names.Count - 1);
                String organizationName = names[oorganizationNameIndex];
                string description = "Helth Service Organization";
                var oorganizationTypeCode = new Random().Next(0, 30);
                int organizationTypeCode = oorganizationTypeCode;
                var ccityCode = new Random().Next(0, 100);
                int cityCode = ccityCode;
                var zzipCode = new Random().Next(0, 1000);
                string zipCode = zzipCode.ToString();
                OrganizationVM SP = new OrganizationVM()
                {

                    MohCode = mohCode,
                    Name = organizationName,
                    Description = description,
                    OrganizationTypeCode = organizationTypeCode,
                    CityCode = cityCode,
                    ZipCode = zipCode
                };
                organizationVMs.Add(SP);



            }
            HttpUtil.Post(uri, organizationVMs);





        }

        [Test]

        public void CreateBranches()
        {
            var queueStore = new QueueStore(ctx);
            ctx = queueStore.ctx;
            string startupPath = Directory.GetCurrentDirectory();

            //  var citeisNames =  File.ReadAllLines(@"c:\users\Barukh\Desktop\example3.txt", Encoding.Default);
            var citiesNames = ctx.Cities.Select(x => x.Name).Take(200).ToArray();
            citiesNames = citiesNames.Where(l => l.Trim().Length > 0).ToArray();
            var branchesList = ctx.Branches;
            var organizations = ctx.Organizations.ToList();
            List<BranchVM> branchVMs = new List<BranchVM>();
            string uri = "Branches";
            foreach (var item in organizations)
            {
                //  if ()
                for (int i = 0; i < 5; i++)
                {


                    var current = new Random().Next(200, 800);
                    int id = current;
                    var orgIdIndex = new Random().Next(0, organizations.Count - 1);
                    int organizationId = organizations[orgIdIndex].Id;
                    var citeisNamesIndex = new Random().Next(0, citiesNames.Length - 1);
                    var citeisnames = citiesNames[citeisNamesIndex];
                    var streetId = new Random().Next(0, 10000);
                    var houseNumber = new Random().Next(0, 1000).ToString();
                    var zipCode = new Random().Next(0, 10000).ToString();
                    var lat = new Random().NextDouble();
                    double lat1 = lat * (30.87548 - 0.5897658);
                    var lng = new Random().NextDouble();
                    double lng1 = lat * (24.87548 - 0.8764784);
                    var phonNumber = new Random().Next(024798478, 088767638).ToString();

                    BranchVM BR = new BranchVM
                    {
                        OrganizationId = organizationId,
                        Name = citeisnames,
                        StreetId = streetId,
                        HouseNumber = houseNumber,
                        ZipCode = zipCode,
                        Lat = (decimal)lat1,
                        Lng = (decimal)lng1,
                        phonNumber = phonNumber

                    };

                    branchVMs.Add(BR);
                    // branchVMs = branchVMs.Take(3).ToList();
                }
            }

            //var numOfOrganizations = ctx.Organizations.Count();
            //var sumOfBranchesToInsert = numOfOrganizations * 5;
            //Assert.IsTrue(sumOfBranchesToInsert == branchVMs.Count);


            HttpUtil.Post(uri, branchVMs);

        }



        [Test]
        public void CreateServiceType()
        {
            var queueStore = new QueueStore(ctx);
            ctx = queueStore.ctx;
            var orgId = ctx.Organizations.Select(x => x.Id).ToList();
            List<ServiceTypeVM> servicetypeVMs = new List<ServiceTypeVM>();
            string uri = "ServiceTypes";
            var serviceTypesNames = File.ReadAllLines(@"c:\users\Barukh\Desktop\example4.txt", Encoding.Default);
            serviceTypesNames = serviceTypesNames.Where(l => l.Trim().Length > 0).ToArray();
            for (int i = 0; i <= serviceTypesNames.Length - 1; i++)
            {
                var orgIdIndex = new Random().Next(0, orgId.Count - 1);
                int organizationId = orgId[orgIdIndex];
                var serviceTypesNamesIndex = new Random().Next(0, serviceTypesNames.Length - 1);
                var description = serviceTypesNames[serviceTypesNamesIndex];
                string code = new Random().Next(0, 100).ToString();
                var cost = new Random().Next(150, 1500);
                var isVisibleToApp = true;
                var isVisibleToOrganization = true;
                ServiceTypeVM ST = new ServiceTypeVM()
                {
                    OrganizationId = organizationId,
                    Description = description,
                    Code = code,
                    Cost = cost,

                };
                servicetypeVMs.Add(ST);
            }
            //var sum = 11;
            //Assert.IsTrue(sum == servicetypeVMs.Count);
            HttpUtil.Post(uri, servicetypeVMs);


        }


        [Test]
        public void CreateSPST()
        {
            List<ServiceProvidersServiceTypeVM> serviceProvidersServiceTypeVM = new List<ServiceProvidersServiceTypeVM>();
            string uri = "ServiceProvidersServiceTypes";
            var queueStore = new QueueStore(ctx);
            ctx = queueStore.ctx;
            var spId = ctx.ServiceProviders.Select(x => x.Id).ToList();
            var stId = ctx.ServiceTypes.Select(x => x.Id).ToList();
            var spIdIndex = new Random().Next(0, spId.Count - 1);
            var stIdIndex = new Random().Next(0, stId.Count - 1);


            for (int i = 0; i < 5; i++)
            {
                var serviceProviderId = spId[spIdIndex];
                var serviceTypeId = stId[stIdIndex];
                var avgDuration = new Random().Next(10, 50);

                ServiceProvidersServiceTypeVM SPST = new ServiceProvidersServiceTypeVM()
                {
                    ServiceProviderId = serviceProviderId,
                    ServiceTypeId = serviceTypeId,
                    AvgDuration = avgDuration
                };
                serviceProvidersServiceTypeVM.Add(SPST);
            }
            //var sum = 5;
            //Assert.IsTrue(sum == serviceProvidersServiceTypeVM.Count);
            HttpUtil.Post(uri, serviceProvidersServiceTypeVM);
        }

        [Test]
        public void CreateTimeTableTest()
        {
            string uri = "TimeTables";
            var queueStore = new QueueStore(ctx);
            ctx = queueStore.ctx;
            var organizationList = ctx.Organizations.ToList();
            var serviceProvidersList = ctx.ServiceProviders.ToList();
            branchesList = ctx.Branches.ToList();
            ttList = ctx.TimeTables.ToList();
            List<int> organizationsIndexes = new List<int>();

            List<TimeTableVM> tables = new List<TimeTableVM>();
            foreach (var serviceProvider in serviceProvidersList)
            {
                if (ttList.Any(svc => svc.ServiceProviderId == serviceProvider.Id))
                    continue;
                var tt = CreateTimeTable(serviceProvider.OrganizationId, serviceProvider.Id);
                if (tt != null)
                {
                    tables.Add(tt);
                }
            }

            HttpUtil.Post(uri, tables);

            var sum = serviceProvidersList.Count() - 1;

            Assert.IsTrue(sum == tables.Count() - 1);

        }

        List<Branch> branchesList;
        List<TimeTable> ttList;
        Random random = new Random(DateTime.Now.Millisecond);

        private TimeTableVM CreateTimeTable(int organizationId, int serviceProviderId)
        {

            var branches = branchesList.Where(y => y.OrganizationId == organizationId);
            Random rnd = new Random();
            if (branches.IsNullOrEmpty())
                return null;
            var branch = branches.ElementAt(rnd.Next(branches.Count()));
            // int branchId = branch.IsNotNull() ? branch.Id : 0;

            //if (branch == null)
            //{
            //    var branchVM = CreateBranch(organizationId);
            //    //  branchId = branchVM.Id;
            //    //return null;
            //}

            var day = random.Next(1, 28);
            var month = random.Next(1, 12);
            var year = random.Next(2018, 2019);
            var ttPeriodInYears = random.Next(1, 3);
            var tempValidFromDate = new DateTime(year, month, day);
            var tempValidTilDate = tempValidFromDate.AddYears(ttPeriodInYears);

            // Here we about to create a few instances of TTL for the TT //

            //var daysOfWork = random.Next(1, 5);
            //var beginHours = random.Next(8, 12);
            //var minutes = random.Next(0, 1) == 1 ? 30 : 0;
            //var secounds = 0;
            //var endHours = random.Next(4, 9);



            //List<TimeTableLine> lines = new List<TimeTableLine>();
            //for (int n = 0; n <= daysOfWork; n++)
            //{
            //    var ttl = CreateTTL(beginHours, minutes, secounds, endHours);
            //    if (!lines.Any(l => (l.WeekDay == ttl.WeekDay)))
            //        lines.Add(ttl);
            //}

            TimeTableVM timeTable = new TimeTableVM()
            {
                ServiceProviderId = serviceProviderId,
                BranchId = branch.Id,
                ValidFromDate = tempValidFromDate,
                ValidUntilDate = tempValidTilDate,
                //   TimeTableLines = lines
            };

            return timeTable;
        }

        //private TimeTableLine CreateTTL(int beginHours, int minutes, int secounds, int endHours)
        //{
        //    var findDay = random.Next(1, 5);
        //    QuzeDayOfWeek d = (QuzeDayOfWeek)findDay;
        //    TimeSpan time1 = new TimeSpan(beginHours, minutes, secounds);
        //    TimeSpan time2 = new TimeSpan(endHours, minutes, secounds);
        //    TimeTableLine ttl = new TimeTableLine(d, time1, time2);
        //    return ttl;
        //}

        //private BranchVM CreateBranch(int organizationId)
        //{

        //    //  var city = ctx.Cities.FirstOrDefault();



        //    //  var citiesNames = ctx.Cities.Take(20).ToList();
        //    //  var citiesNames = ctx.Cities.Select(n => n.Name).Take(20).ToArray();
        //    //  citiesNames = citiesNames.Where(l => l.Trim().Length > 0).ToArray();
        //    List<BranchVM> branchVMs = new List<BranchVM>();
        //    string url = "Branches";


        //    var citeisNamesIndex = random.Next(0, citiesNames.Count - 1);
        //    var cityName = citiesNames[citeisNamesIndex].Name;
        //    //  var citeisnames = citiesNames[citeisNamesIndex];
        //    var streetId = random.Next(0, 10000);
        //    var houseNumber = random.Next(0, 1000).ToString();
        //    var zipCode = random.Next(0, 10000).ToString();
        //    var lat = random.NextDouble();
        //    double lat1 = lat * (30.87548 - 0.5897658);
        //    //  var lng = random.NextDouble();
        //    double lng1 = lat * (24.87548 - 0.8764784);
        //    //  var phonNumber = random.Next(024798478, 088767638).ToString();

        //    BranchVM branch = new BranchVM
        //    {
        //        OrganizationId = organizationId,
        //        Name = cityName,
        //        StreetId = streetId,
        //        HouseNumber = houseNumber,
        //        ZipCode = zipCode,
        //        Lat = lat1,
        //        Lng = lng1,

        //    };

        //    branchVMs.Add(branch);
        //    HttpUtil.Post(url, branchVMs);

        //    return branch;
        //}

        [Test]
        public void CreateTimeTableLine()
        {
            string uri = "TimeTableLines";
            var queueStore = new QueueStore(ctx);
            ctx = queueStore.ctx;
            var timeTableDraw = ctx.TimeTables.ToList();
            var timeTableLineDraw = ctx.TimeTableLines.ToList();
            Random random = new Random(DateTime.Now.Millisecond);
            List<TimeTable> timeTableVMs = new List<TimeTable>();
            List<TimeTableLine> TTL = new List<TimeTableLine>();
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
            HttpUtil.Post(uri, timeTableDraw);


        }
    }
}






