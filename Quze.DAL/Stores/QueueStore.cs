using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ML;
using Quze.Infrastruture.Extensions;
using Quze.Models;
using Quze.Models.Entities;
using Quze.Models.Logic;
using Quze.Models.ML;
using Quze.Models.Models;

namespace Quze.DAL.Stores
{
    /// <summary>
    /// DAL for service queues, returns from cache and works against the DB
    /// </summary>
    public class QueueStore : StoreBase<ServiceQueue>
    {
        private static int MaxServiceQ2Fetch { get; } = 5;
        private readonly GetEntitiesTemporary getEntities;
        private const int MaxSlots2Fetch = 20;

        public QueueStore(QuzeContext ctx) : base(ctx)
        {
            getEntities = new GetEntitiesTemporary(ctx);
        }

        

        ///// <summary>
        ///// Returns an existing queue for the given date AND TIME or a new one if it does not exists
        ///// <param name="dateTime">This parameter includes TIME </param>
        ///// <returns></returns>
        ///// </summary>
        //private async Task<List<ServiceQueue>> CreateServiceQueuesByTt(TimeTable timeTable, DateTime dateTime,
        //    DateTime endDateTime, int arrivalTime, int serviceQ2Fetch = 0)
        //{
        //    var maxDate = endDateTime;
        //    if (timeTable.ValidUntilDate.Date < endDateTime.Date)
        //        maxDate = timeTable.ValidUntilDate.Date;
        //    ////CheckDate(ref dateTime);
        //    //var serviceQueues = await GetExistingSQAsync(serviceProviderId, branchId, dateTime, false);
        //    serviceQ2Fetch = serviceQ2Fetch == 0 ? MaxServiceQ2Fetch : serviceQ2Fetch;
        //    var serviceQueues = new List<ServiceQueue>();
        //    ////TODO: check that there is available slots in the service queues

        //    if (timeTable.IsNull() || timeTable.TimeTableLines.IsNullOrEmpty())
        //        return null;

        //    timeTable.OrderTableLinesByNearestDateTime(dateTime);
        //    var weeksFromDate = 0;
        //    var nextTtl = timeTable.TimeTableLines.Where(ttl => (int)ttl.WeekDay >= (int)dateTime.QuzeDayOfWeek());
        //    var prevTtl = nextTtl.Any() ?
        //        timeTable.TimeTableLines[0] :
        //        timeTable.TimeTableLines[timeTable.TimeTableLines.Count - 1];
        //    var date = DateTime.MinValue;

        //    while (serviceQueues.Count < serviceQ2Fetch && date.Date <= maxDate)
        //    {
        //        foreach (var ttl in timeTable.TimeTableLines)
        //        {
        //            date = CalculateDate(dateTime, arrivalTime, ref weeksFromDate, ref prevTtl, ttl);
        //            if (date.Date > maxDate)
        //                break;

        //            var sq = new ServiceQueue(date, null, date.TimeOfDay.GetDuration(ttl.EndTime))
        //            {
        //                ServiceProviderId = timeTable.ServiceProviderId,
        //                BranchId = timeTable.BranchId,
        //                Branch = timeTable.Branch,
        //                ServiceProvider = timeTable.ServiceProvider,
        //                ServiceTypeId = timeTable.ServiceTypeId
        //            };


        //            if (getEntities.IsVacationDate(sq.BeginTime.Date, timeTable.ServiceProviderId, ctx))
        //                continue;

        //            sq = getEntities.ChangeSqByException(sq, ctx.TimeTableExceptions, ttl);
        //            serviceQueues.Add(sq);
        //        }

        //    }


        //    //in sq of today, slots starting no earlier than now
        //    foreach (var sq in serviceQueues)
        //    {
        //        if (sq.BeginTime >= DateTime.Now.AddSeconds(arrivalTime))
        //            continue;
        //        sq.BeginTime = DateTime.Now.ZeroMilliseconds().AddSeconds(arrivalTime);

        //        if (sq.BeginTime > sq.EndTime)
        //            sq.EndTime = sq.BeginTime;///////////TODO what is the meaning of this?
        //    }
        //    return serviceQueues;
        //}

        public IEnumerable<TimeTable> SchdulePlanningData(int serviceProviderId, int branchId, DateTime dateTime, Branch branch, ServiceProvider serviceProvider, DateTime endDate, bool timeTableOnly)
        {
            var dates = new List<DateTime>();
            var startOfWeek = dateTime.AddDays(0 - (int)dateTime.DayOfWeek);
            endDate = endDate == dateTime ? startOfWeek.AddDays(6) : endDate.AddDays(6 - (int)endDate.DayOfWeek);
            do
            {
                dates.Add(startOfWeek);
                startOfWeek = startOfWeek.AddDays(7);

            } while (startOfWeek < endDate);
            startOfWeek = dateTime.AddDays(0 - (int)dateTime.DayOfWeek);

            //DateTime startOfWeek = dateTime.AddDays(0 - (int)dateTime.DayOfWeek);

            //  var serviceQueues = new List<ServiceQueue>();

            var timeTables = getEntities.GetTimeTables(serviceProviderId, branchId, startOfWeek, endDate, ctx);

            if (!timeTables.IsNull())
            {
                var numNull = 0;
                for (; numNull < timeTables.Count; numNull++)
                {
                    if (!timeTables[numNull].TimeTableLines.IsNullOrEmpty())
                        break;
                }
                if (numNull == timeTables.Count)
                    return null;
            }
            else
                return null;

            foreach (var timeTable in timeTables)
                timeTable.OrderTableLinesByNearestDateTime(startOfWeek);

            foreach (var timeTable in timeTables)
                foreach (var ttl in timeTable.TimeTableLines)
                    ttl.TimeTable = null;

            return timeTables;
        }

        //TODO: function not clear
        public TimeTable CreateTimeTableLine(TimeTable newTt)
        {
            var tt = getEntities.GetTimeTable(newTt.ServiceProviderId, newTt.BranchId, newTt.ValidFromDate, newTt.ValidUntilDate, ctx);
            if (tt == null)
            {                                     //not exist TT
                ctx.TimeTables.AddRange(newTt);
                ctx.SaveChanges();

                return newTt;
            }

            //exist TT
            tt.TimeTableLines.AddRange(newTt.TimeTableLines);
            ctx.Entry(tt).CurrentValues.SetValues(tt);
            ctx.SaveChanges();
            return newTt;

        }

        public IEnumerable<ServiceQueue> GetCalendarData(int serviceProviderId, int branchId, DateTime dateTime, int arrivalTime, Branch branch, ServiceProvider serviceProvider, DateTime endDate)
        {
            var dates = new List<DateTime>();
            var startOfWeek = dateTime.AddDays(-(int)dateTime.DayOfWeek);
            endDate = endDate == dateTime ? startOfWeek.AddDays(6) : endDate.AddDays(6 - (int)endDate.DayOfWeek);
            do
            {
                dates.Add(startOfWeek);
                startOfWeek = startOfWeek.AddDays(7);

            } while (startOfWeek < endDate);

            startOfWeek = dateTime.AddDays(-(int)dateTime.DayOfWeek);

            IEnumerable<ServiceQueue> serviceQueues = new List<ServiceQueue>();

            foreach (var date in dates)
                serviceQueues = GetExistingSq(serviceProviderId, branchId, date);


            var timeTables = getEntities.GetTimeTables(serviceProviderId, branchId, startOfWeek, endDate, ctx);

            if (!timeTables.IsNull())
            {
                var numNull = 0;
                for (; numNull < timeTables.Count; numNull++)
                {
                    if (!timeTables[numNull].TimeTableLines.IsNullOrEmpty())
                        break;
                }
                if (numNull == timeTables.Count)
                    return null;
            }
            else
                return null;
            foreach (var timeTable in timeTables)
            {
                timeTable.OrderTableLinesByNearestDateTime(startOfWeek);
            }

            var weeksFromDate = 0;
            serviceQueues = serviceQueues.ToList();

            foreach (var timeTable in timeTables)
            {
                var prevTtl = timeTable.TimeTableLines[0];
                var date = DateTime.MinValue;
                while (date.IsBetween(timeTable.ValidFromDate, timeTable.ValidUntilDate) && date < endDate)
                    foreach (var ttl in timeTable.TimeTableLines)
                    {
                        date = CalculateDate(startOfWeek, arrivalTime, ref weeksFromDate, ref prevTtl, ttl, true);
                        if (date > timeTable.ValidUntilDate || date > endDate)
                        {
                            break;
                        }
                        var sq = new ServiceQueue(date, null, date.TimeOfDay.GetDuration(ttl.EndTime))
                        {
                            TimeTableId = timeTable.Id,
                            ServiceProviderId = serviceProviderId,
                            BranchId = timeTable.BranchId,
                            Branch = timeTable.Branch,
                            ServiceProvider = serviceProvider
                        };

                        if (getEntities.IsVacationDate(sq.BeginTime.Date, serviceProviderId, ctx))
                            continue;

                        sq = getEntities.ChangeSqByException(sq, ctx.TimeTableExceptions, ttl);

                        if (!serviceQueues.Contains(sq))
                            serviceQueues.Concat(new[] { sq });
                    }

            }
            return serviceQueues;
        }

        private static DateTime CalculateDate(DateTime dateTime, int arrivalTime, ref int weeksFromDate, ref TimeTableLine prevTtl, TimeTableLine ttl, bool calendarData = false)
        {
            if (prevTtl.WeekDay > ttl.WeekDay) //next week another day
                weeksFromDate++;

            if (prevTtl != ttl && prevTtl.WeekDay == ttl.WeekDay && prevTtl.BeginTime >= ttl.BeginTime)//next week same day
                weeksFromDate++;

            prevTtl = ttl;

            var daysDifference = weeksFromDate * 7 + (int)ttl.WeekDay - (int)dateTime.QuzeDayOfWeek();
            var date = dateTime.AddDays(daysDifference).Date;

            date = date.Add(ttl.BeginTime);

            if (!calendarData && date.Date == DateTime.Now.Date && date <= DateTime.Now)
                date = DateTime.Now.ZeroMilliseconds().AddSeconds(arrivalTime);

            return date;
        }

        private IEnumerable<ServiceQueue> GetExistingSq(int serviceProviderId, int branchId, DateTime dateTime)
        {
            var existingSq = ctx.ServiceQueues.Where(q => q.ServiceProviderId == serviceProviderId
                                               && (branchId == 0 || q.BranchId == branchId)
                                               && q.BeginTime.Date >= dateTime.Date && q.BeginTime.Date <= dateTime.Date.AddDays(6)
                                               && q.IsDeleted == false
                                              )
                                            .Include(s => s.ServiceType)
                                              .Include(s => s.ServiceProvider).AsQueryable().Include(s => s.Appointments).ThenInclude(a => a.Fellow).Include(s => s.Branch);

            foreach (var sq in existingSq)
                sq.Appointments.Sort((x, y) => x.BeginTime.CompareTo(y.BeginTime));

            return existingSq;
        }

        public GetAvailableSlotsResponse GetAvailableSlotsBySt(GetAvailableSlotsRequest request)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var response = new GetAvailableSlotsResponse();
            if (request.ServiceTypeId.IsNull() && request.ServiceProviderId.IsNull())
            {
                response.AddError(232, "Please select service provider or service type");
                return response;
            }

            //  List<TimeTable> timeTables;

            //0. get relevant time tables

            //  var timeTablesQuery = GetTimeTablesByUserRequest(request);
            //  timeTables = timeTablesQuery.ToList();


            //if (timeTables.IsNullOrEmpty())
            //{
            //    response.AddError(232, "no Time Tables found");
            //    return response;
            //}

            var after0 = watch.Elapsed;///////////////////

            Fellow fellow = null;
            var fellowStore = new FellowStore(ctx);

            if (request.FellowId != null)
            {
                fellow = fellowStore.GetByIdAsync(request.FellowId.Value).Result;
            }




            var endTime = request.EndTime ?? DateTime.MaxValue;
            List<ServiceQueue> serviceQueuesFromDb;
            //1.get existing sq from db
            try
            {
                //serviceQueuesFromDb = getEntities.GetExistingSq(request.BeginTime, request.ArrivalTime,
                //   MaxServiceQ2Fetch, request.ServiceProviderId ?? default(int), request.BranchId ?? default(int)).ToListAsync().Result;
                serviceQueuesFromDb = GetServiceQueueByUserRequest(request).ToList();
                int numOfSq = serviceQueuesFromDb.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            var after1 = watch.Elapsed;//////////////////////////////
                                       //      if (serviceQueuesFromDb.IsNullOrEmpty())
                                       //         throw new ArgumentNullException(nameof(serviceQueuesFromDb));

            //2.build MaxServiceQ2Fetch closed SQ by tt
            //var timeTableLogic = new TimeTableLogic(timeTables, ctx.TimeTableExceptions, ctx.TimeTableVacations, MaxServiceQ2Fetch);
            //var serviceQueuesByTimeTables = timeTableLogic.CreateSQsInRangeOfDatesByTTs(request.BeginTime, endTime).ToList();

            var after2 = watch.Elapsed;//////////////////////////////
                                       //3.synchronize the two list(steps 1 and 2)
                                       //  var serviceQueues = serviceQueuesFromDb.Union(serviceQueuesByTimeTables).ToList();
                                       //  var serviceQueues = serviceQueuesFromDb;
            serviceQueuesFromDb.Sort();
            var after3 = watch.Elapsed;//////////////////////////////
                                       //4.get slots by ServiceQueue
            response.Entities = GetSpBranchSlots(request, serviceQueuesFromDb, fellow).Take(20).ToList();
            var after4 = watch.Elapsed;/////////////////////////////////
            return response;
        }

        public GetAvailableSlotsResponse GetAvailableSlotsToSQ(GetAvailableSlotsRequest request)
        {
            var response = new GetAvailableSlotsResponse();
            if (request.ServiceQueue == null)
            {
                response.AddError(233, "Please select Service Queue");
                return response;
            }
            TimeTable TimeTable = ctx.TimeTables.Include(b => b.Branch)
                .Include(sp => sp.ServiceProvider)
                .Include(tt => tt.ServiceType)
                .Include(t => t.TimeTableLines).FirstOrDefault(t => t.Id == request.ServiceQueue.TimeTableId);
            if (TimeTable == null)
            {
                response.AddError(234, "Time table not found.");
                return response;
            }
            Fellow fellow = ctx.Fellows.FirstOrDefault(f => f.Id == request.FellowId);
            var spBSlot = new SP_BranchSlots(TimeTable);

            spBSlot.Slots = GetSP_BranchesSlotsForSqs(new List<ServiceQueue>() { request.ServiceQueue }
                , request.ArrivalTime, fellow, request.Duration ?? 0).ToList();
            //assuming:service provider has only one service type.
            ServiceType serviceType = GetServiceTypeBySErviceProvider(TimeTable.ServiceProviderId);
            spBSlot.ServiceType = serviceType != null ? serviceType : request.ServiceQueue.ServiceType;
            response.Entities = new List<SP_BranchSlots>() { spBSlot };
            return response;
        }

        private IEnumerable<SP_BranchSlots> GetSpBranchSlots(GetAvailableSlotsRequest request, IEnumerable<TimeTable> timeTables, IReadOnlyCollection<ServiceQueue> serviceQueues, Fellow fellow)
        {

            foreach (var sq in serviceQueues)
            {
                var spBSlot = new SP_BranchSlots(sq);
                spBSlot.Slots = GetSP_BranchesSlotsForSqs(
                    new List<ServiceQueue> { sq }, request.ArrivalTime, fellow, request.Duration ?? 0).ToList();
                spBSlot.ServiceType = GetServiceTypeBySErviceProvider(sq.ServiceProviderId);
                yield return spBSlot;
            }

            foreach (var tt in timeTables)
            {
                var spBSlot = new SP_BranchSlots(tt);
                var serviceQsOfSpAndBranch = serviceQueues.FilterByBranches(new[] { tt.Branch })
                    .FilterSqByServiceProviders(new[] { tt.ServiceProvider });
                spBSlot.Slots = GetSP_BranchesSlotsForSqs(
                    serviceQsOfSpAndBranch, request.ArrivalTime, fellow, request.Duration ?? 0).ToList();
                //assuming:service provider has only one service type.
                spBSlot.ServiceType = GetServiceTypeBySErviceProvider(tt.ServiceProviderId);
                yield return spBSlot;
            }
        }


        private IEnumerable<SP_BranchSlots> GetSpBranchSlots(GetAvailableSlotsRequest request, List<ServiceQueue> serviceQueues, Fellow fellow)
        {

            var serviceQueuesList = serviceQueues.ToList();

            var groupedServiceQueues = from sq in serviceQueuesList
                                       group sq by sq.TimeTableId into g
                                       select new { TimeTableId = g.Key, ServiceQueues = g.ToList() };

            foreach (var sQs in groupedServiceQueues)
            {
                var spBSlot = new SP_BranchSlots(sQs.ServiceQueues.FirstOrDefault());
                spBSlot.Slots = GetSP_BranchesSlotsForSqs(
                   sQs.ServiceQueues, request.ArrivalTime, fellow, request.Duration ?? 0).Take(5).ToList();
                //  spBSlot.ServiceType = GetServiceTypeBySErviceProvider(sq.ServiceProviderId);
                if (spBSlot.Slots.Count > 0)
                    yield return spBSlot;
            }
        }

        private ServiceType GetServiceTypeBySErviceProvider(int serviceProviderId)
        {

            var serviceTypesList = ctx.ServiceProvidersServiceTypes
                .Include(spst => spst.ServiceType)
                .Where(spst => spst.ServiceProviderId == serviceProviderId)
                .OrderBy(spst => spst.ServiceTypeId)
                .Select(spst => spst.ServiceType);
            ServiceType ST = serviceTypesList.Where(st => st.IsVisibleToOrganization == true).FirstOrDefault();
            //ST.ServiceProvidersServiceTypes = null;
            return ST;
        }


        private IQueryable<ServiceQueue> GetServiceQueueByUserRequest(GetAvailableSlotsRequest request)
        {
            var sqQuery = getEntities.GetSqWithIncludes()
                .IsBetween(request.BeginTime, request.EndTime ?? DateTime.MaxValue)
                .FilterSqByServiceProviderId(request.ServiceProviderId)
                //.FilterSqByServiceTypeId(request.ServiceTypeId)
                .FilterSqByBranchId(request.BranchId)
               .FilterByOrganizationId(request.OrganizationId ?? default(int));

            if (request.ServiceTypeId.IsNotNull()&& request.ServiceTypeId!=1)
            {
                var qur2 = ctx.ServiceProvidersServiceTypes.Where(spst => spst.ServiceTypeId == request.ServiceTypeId).Select(spst => spst.ServiceProviderId).ToList();

                sqQuery = sqQuery.Where(sq => qur2.Contains(sq.ServiceProviderId));
            }

            return sqQuery;
        }

        private IQueryable<TimeTable> GetTimeTablesByUserRequest(GetAvailableSlotsRequest request)
        {
            var timeTablesQuery = getEntities.GetTimeTablesWithIncludes()
                .FilterByCurrentValidity(request.BeginTime)
                .FilterByServiceProviderId(request.ServiceProviderId)
                .FilterByServiceTypeId(ctx, request.ServiceTypeId)
                .FilterByBranchId(request.BranchId)
                .FilterByOrganizationId(request.OrganizationId ?? default(int), ctx);
            return timeTablesQuery;
        }

        private IEnumerable<Slot> GetSP_BranchesSlotsForSqs(IEnumerable<ServiceQueue> serviceQueues, int arrivalTime, Fellow fellow, int duration)
        {
            var serviceQueuesList = serviceQueues.ToList();
            foreach (var sq in serviceQueuesList)
            {
                var sqDuration = GetDurationFromDb(sq.ServiceProviderId, sq.ServiceTypeId.GetValueOrDefault());
                sqDuration = new List<int> { sqDuration, duration }.Max();
                var serviceQueueLogic = new ServiceQueueLogic(sq);
                var slots = serviceQueueLogic.GetServiceQueueSlots(arrivalTime, fellow, sqDuration)
                    .Where(s => s.BeginTime.Date != DateTime.Today || s.BeginTime.TimeOfDay > DateTime.Now.TimeOfDay);

                foreach (var slot in slots)
                    yield return slot;
            }
        }

        private int GetDurationFromDb(int serviceProviderId, int serviceTypeId)
        {
            var serviceProviderServiceType = ctx.ServiceProvidersServiceTypes.FirstOrDefault(spSt => spSt.ServiceProviderId == serviceProviderId && spSt.ServiceTypeId == serviceTypeId);

            if (serviceProviderServiceType != null && serviceProviderServiceType.AvgDuration > 0)
                return serviceProviderServiceType.AvgDuration;
            return 15 * 60;
        }

        //private List<ServiceQueue> SplitServiceQueueByTime(ServiceQueue sq)
        //{
        //    var serviceQueues = new List<ServiceQueue>();

        //    if (sq.BeginTime.GetTimeInDay() == TimeInDay.Morning && sq.EndTime.GetTimeInDay() == TimeInDay.Morning
        //        || sq.BeginTime.GetTimeInDay() == TimeInDay.AfterNoon && sq.EndTime.GetTimeInDay() == TimeInDay.AfterNoon
        //        || sq.BeginTime.GetTimeInDay() == TimeInDay.Evening)
        //    {
        //        serviceQueues.Add(sq);
        //        return serviceQueues;
        //    }

        //    if (sq.BeginTime.GetTimeInDay() == TimeInDay.Morning)
        //    {
        //        serviceQueues.Add(new ServiceQueue(sq, sq.BeginTime, sq.BeginTime.Date.AddHours(12)));
        //        if (sq.EndTime.GetTimeInDay() == TimeInDay.AfterNoon)
        //        {
        //            serviceQueues.Add(new ServiceQueue(sq, sq.BeginTime.Date.AddHours(12), sq.EndTime));
        //            return serviceQueues;
        //        }
        //        serviceQueues.Add(new ServiceQueue(sq, sq.BeginTime.Date.AddHours(12), sq.BeginTime.Date.AddHours(16)));
        //        serviceQueues.Add(new ServiceQueue(sq, sq.BeginTime.Date.AddHours(16), sq.EndTime));
        //        return serviceQueues;
        //    }

        //    if (sq.BeginTime.GetTimeInDay() == TimeInDay.AfterNoon)
        //    {
        //        serviceQueues.Add(new ServiceQueue(sq, sq.BeginTime, sq.BeginTime.Date.AddHours(16)));
        //        serviceQueues.Add(new ServiceQueue(sq, sq.BeginTime.Date.AddHours(16), sq.EndTime));
        //        return serviceQueues;
        //    }

        //    return serviceQueues;
        //}

        public IEnumerable<ServiceQueue> GetCurrentQuze(int branchId)
        {
            return getEntities.GetServiceQueuesWithProps(ctx).Where(sq => sq.BranchId == branchId
                                                                           && sq.BeginTime.Date == DateTime.Now.Date
                                                                           && sq.Passed == false
            );
        }


        private ServiceQueue CreateServiceQueue(int serviceProviderId, int branchId, DateTime beginTime)
        {
            var timeTable = getEntities.GetTimeTable(serviceProviderId, branchId, beginTime, ctx);

            if (timeTable.IsNull() || timeTable.TimeTableLines.IsNullOrEmpty())
                return null;

            var ttl = timeTable[beginTime];
            if (ttl.IsNull() || ttl.BeginTime > beginTime.TimeOfDay)
                return null;

            return new ServiceQueue(ttl, beginTime.Date);
        }

        public async Task<Appointment> SaveAppointmentAsync(int fellowId, int serviceTypeId, int serviceProviderId, int branchId, DateTime beginTime, DateTime fellowBirthdate, int arrivalTime, int? serviceQueueId)
        {
            //*********         Get Duration from ML
            var request = new RequestML()
            {
                ServiceProviderId = serviceProviderId,
                FellowId = fellowId,
                AppointmentTime = beginTime,
                ServiceTypeId = serviceTypeId,
                FellowBirthDateAndTime = fellowBirthdate
            };
            var response = await new Duration().GetDurationAsync(request);
            var duration = response.responseDuration.Value;
            //**********            End ML

            ServiceQueue serviceQueue;
            if (serviceQueueId != null && serviceQueueId.Value > 0)
            {
                serviceQueue = await getEntities.GetServiceQueueWithAppointmentsAsync(serviceQueueId.Value, ctx);

                if (serviceQueue.IsNull())
                {
                    throw new ApplicationException($"ServiceQueueId {serviceQueueId.Value} does not exist");
                }
            }
            else
            {
                serviceQueue = await ctx.ServiceQueues
                    .Include(sq => sq.ServiceProvider)
                    .Include(sq => sq.Appointments)
                    .Include(sq => sq.Branch)
                    .Where(x =>
                       x.ServiceProviderId == serviceProviderId
                       && x.BranchId == branchId
                       && x.BeginTime <= beginTime
                       && beginTime <= x.EndTime
                    ).FirstOrDefaultAsync();
            }

            if (serviceQueue.IsNotNull())
            {
                var serviceQueueLogic = new ServiceQueueLogic(serviceQueue);
                var slots = serviceQueueLogic.GetServiceQueueSlots(arrivalTime, null, -1);

                var slot = slots.FirstOrDefault(s => beginTime.IsBetween(s.BeginTime, s.EndTime) &&
                                                     beginTime.AddSeconds(duration).IsBetween(s.BeginTime, s.EndTime));
                if (slot == default(Slot))//check if there is slot in 5 minutes after begin time
                {
                    slot = slots.FirstOrDefault(s => beginTime.AddSeconds(300).IsBetween(s.BeginTime, s.EndTime)
                                                     && (s.EndTime - s.BeginTime).TotalSeconds >= duration);
                    if (slot != default(Slot))
                        beginTime = slot.BeginTime;
                    else
                    {
                        var e = new ApplicationException("The appointment you requested is not available ");
                        e.Data.Add("duration", duration);
                        throw e;
                    }
                }



                if (!beginTime.IsBetween(serviceQueue.BeginTime, serviceQueue.EndTime))
                    throw new ApplicationException("the time of the appointment are not matching with the service queue");

            }
            else//service queue does not exist
            {
                serviceQueue = CreateServiceQueue(serviceProviderId, branchId, beginTime);//TODO: GetServiceQueue should return single object
                if (serviceQueue.IsNull())
                {
                    throw new ApplicationException(
                        $"No time table matches for service provider {serviceProviderId} and branch {branchId} for the appointment requested time: {beginTime}");
                }

                await ctx.ServiceQueues.AddAsync(serviceQueue);
                await ctx.SaveChangesAsync();
            }



            var endTime = beginTime.AddSeconds(duration);


            var appointment = new Appointment
            {
                FellowId = fellowId,
                ServiceTypeId = serviceTypeId,
                ServiceQueue = serviceQueue,
                ServiceQueueId = serviceQueue.Id,
                BeginTime = beginTime,
                Duration = duration,
                EndTime = endTime
            };

            await ctx.Appointments.AddAsync(appointment);

            await ctx.SaveChangesAsync();

            await AddMinimalKit(appointment);
            var res = await ctx.Appointments.Include(a => a.ServiceType)
                .Include(a => a.ServiceQueue)
                .ThenInclude(sq => sq.Branch)
                .ThenInclude(o => o.Organization)
                .Include(b => b.ServiceQueue.Branch)
                .ThenInclude(s => s.Street)
                .FirstOrDefaultAsync(a => a.Id == appointment.Id);
            return res;
        }

        private async Task AddMinimalKit(Appointment appointment)
        {
            try
            {
                var requiredTasksQuery = ctx.RequiredTasks.Where(x => x.ServiceTypeID == appointment.ServiceTypeId);
                var requiredDocumentsQuery = ctx.RequiredDocuments.Where(x => x.ServiceTypeID == appointment.ServiceTypeId);
                Task.WaitAll(requiredTasksQuery.ToArrayAsync(), requiredDocumentsQuery.ToArrayAsync());
                foreach (var task in requiredTasksQuery)
                {
                    var appointmentTask = new AppointmentTask { AppointmentId = appointment.Id, RequiredTaskId = task.Id };
                    await ctx.AppointmentTasks.AddAsync(appointmentTask);
                }
                foreach (var doc in requiredDocumentsQuery)
                {
                    var appointmentDoc = new AppointmentDocument { AppointmentId = appointment.Id, RequiredDocumentId = doc.Id };
                    await ctx.AppointmentDocuments.AddAsync(appointmentDoc);
                }
                await ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ServiceQueueLogic>> GetUserAppointmentsServiceQ(User user)
        {
            var serviceQueuesLogic = new List<ServiceQueueLogic>();
            var now = Time.GetNow();
            try
            {
                var fellowIds = user.Fellows.Select(f => f.Id).ToList();

                var serviceQueuesIdsQuery = ctx.Appointments
                    .Where(a =>
                  fellowIds.Contains(a.FellowId)
                    && a.Served == false
                    && a.BeginTime.Date >= now.Date).Select(a => a.ServiceQueueId);

                var serviceQueuesQuery = ctx.ServiceQueues
                    .OrderBy(q => q.BeginTime)
                .Include(sq => sq.Appointments)
                .ThenInclude(a => a.Fellow)
                 .Include(sq => sq.Appointments)
                .ThenInclude(q => q.AppointmentDocs).ThenInclude(ad => ad.RequiredDocument)
                 .Include(sq => sq.Appointments)
                .ThenInclude(q => q.AppointmentTasks).ThenInclude(ad => ad.RequiredTask)
                .Include(sq => sq.Appointments).
                ThenInclude(q => q.ServiceType)
                .Include(q => q.ServiceProvider)
                .Include(q => q.ServiceStation)
                .Include(s => s.CurrentAppointement)
                .Include(s => s.Branch)
                .ThenInclude(b => b.Street)
                .Include(s => s.Branch)
                .ThenInclude(s => s.Organization)
                .Where(sq => serviceQueuesIdsQuery.Contains(sq.Id));
                


                Parallel.ForEach(serviceQueuesQuery, sq =>
                {
                    var logic = new ServiceQueueLogic(sq);
                    logic.CalculatePositionInQueue();
                    logic.CalculateDelay();
                    logic.CalculateEtts();
                    logic.CalculateNextPush();
                    serviceQueuesLogic.Add(logic);
                });


            }
            catch (Exception ex)
            {

                throw;
            }
            return serviceQueuesLogic;
        }

    }

}




