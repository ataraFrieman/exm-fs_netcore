using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;

namespace Quze.DAL
{

    public class GetEntitiesTemporary
    {
        public static List<ServiceType> ServiceTypesOfDb { get; set; }

        private QuzeContext ctx;
        public GetEntitiesTemporary(QuzeContext ctx)
        {
           this.ctx = ctx;
        }

        public TimeTable GetTimeTable(int serviceProviderId, int branchId, DateTime dateTime, DateTime endDate, QuzeContext ctx)
        {
            return ctx.TimeTables.Where(tt => tt.ServiceProviderId == serviceProviderId
                                              && (branchId == 0 || tt.BranchId == branchId)
                                              && tt.ValidFromDate == dateTime
                                              && tt.ValidUntilDate == endDate)
                .Include(tt => tt.TimeTableLines)
                .Include(tt => tt.Branch)
                .Include(tt => tt.ServiceProvider)
                .FirstOrDefault();
        }

        public List<TimeTable> GetTimeTables(int serviceProviderId, int branchId, DateTime beginDate, DateTime endDate, QuzeContext
            ctx)
        {
            return ctx.TimeTables
                .Where(tt => tt.ServiceProviderId == serviceProviderId
                             && (branchId == 0 || tt.BranchId == branchId)
                             // TODO: the condition here not clear
                             && (beginDate >= tt.ValidFromDate && beginDate <= tt.ValidUntilDate ||
                                 (endDate.IsBetween(tt.ValidFromDate, tt.ValidUntilDate)) ||
                                 (tt.ValidFromDate >= beginDate && tt.ValidFromDate <= endDate)))

                .Include(tt => tt.TimeTableLines)
                .Include(tt => tt.Branch)
                .Include(tt => tt.ServiceProvider)
                .ToList();
        }

        public TimeTable GetTimeTable(int serviceProviderId, int branchId, DateTime beginDate, QuzeContext ctx)
        {
            return ctx.TimeTables.Where(tt => tt.ServiceProviderId == serviceProviderId
                                              && (branchId == 0 || tt.BranchId == branchId)
                                              && beginDate >= tt.ValidFromDate && beginDate.Date <= tt.ValidUntilDate)
                .Include(tt => tt.TimeTableLines)
                .Include(tt => tt.Branch)
                .Include(tt => tt.ServiceProvider)
                .FirstOrDefault();
        }

        public IQueryable<ServiceQueue> GetExistingSq
            ( DateTime dateTime, int arrivalTime, int numOfResults, int serviceProviderId = 0, int branchId = 0)
        {
            var serviceQueues = ctx.ServiceQueues.Where(sq =>
                    (serviceProviderId == 0 || sq.ServiceProviderId == serviceProviderId)
                    && (branchId == 0 || sq.BranchId == branchId)
                    && sq.EndTime > dateTime.AddSeconds(arrivalTime)
                    && sq.Passed == false
                ).Include(s => s.ServiceType)
                .Include(s => s.Branch)
                .Include(s => s.ServiceProvider)
                .Include(s =>s.Appointments)
                .OrderBy(s => s.BeginTime)
                .Take(numOfResults);

            
            return serviceQueues;
        }

        public bool IsVacationDate(DateTime date, int serviceProviderId, QuzeContext ctx)
        {
            return ctx.TimeTableVacations.Any(x => x.ServiceProviderId == serviceProviderId
                                                   && date.IsBetween(x.BeginTime, x.EndTime));
        }

        /// <summary>
        /// returns SQ with all appointments
        /// </summary>
        /// <param name="id"></param>
        public async Task<ServiceQueue> GetServiceQueueWithAppointmentsAsync(int id, QuzeContext ctx)
        {
            return await ctx.ServiceQueues
                .Include(sq => sq.Appointments)
                .Include(sq => sq.Branch)
                .Include(sq => sq.ServiceProvider)
                .FirstOrDefaultAsync(sq => sq.Id == id);
        }

        public ServiceQueue ChangeSqByException(ServiceQueue sq, IEnumerable<TimeTableException> exceptionsSource, TimeTableLine ttl)
        {
            var exception = exceptionsSource.FirstOrDefault(exp => exp.TimeTableId == ttl.Id && exp.DateTime.Date == sq.BeginTime.Date);

            if (exception == null)
                return sq;
            sq.BeginTime = exception.BeginTime;
            sq.EndTime = exception.EndTime;

            return sq;
        }

        public IEnumerable<ServiceQueue> GetServiceQueuesWithProps(QuzeContext ctx)
        {
            return ctx.ServiceQueues
                .Include(sq => sq.ServiceProvider)
                .Include(sq => sq.Appointments).ThenInclude(a => a.Fellow)
                .Include(sq => sq.ServiceType)
                .Include(sq => sq.ServiceStation);
        }

        public IQueryable<TimeTable> GetTimeTablesWithIncludes()
        {
            //TODO: check validity dates of TT
            IQueryable<TimeTable> timeTablesQuery = ctx.TimeTables
                .Include(b => b.Branch)
                .Include(sp => sp.ServiceProvider)
                .Include(t => t.TimeTableLines);
            return timeTablesQuery;
        }

        public IQueryable<Branch> GetOrganizationBranches(Organization organization, QuzeContext ctx)
        {
            var branches = ctx.Branches.Where(b => b.OrganizationId == organization.Id);
            return branches;
        }

        public IQueryable<Branch> GetOrganizationBranches(int organizationId)
        {
            var organization = ctx.Organizations.FirstOrDefault(o => o.Id == organizationId);
            var branches = GetOrganizationBranches(organization, ctx);
            return branches;
        }

        public IQueryable<ServiceQueue> GetSqWithIncludes()
        {
            IQueryable<ServiceQueue> queuesQueryable = ctx.ServiceQueues
                .Include(b => b.Branch)
                .ThenInclude(b=> b.Organization)
                .Include(b => b.Branch)
                .ThenInclude(b => b.Street)
                .Include(sp => sp.ServiceProvider)
                .Include(t => t.Appointments)
                .Include(t => t.ServiceType);
            return queuesQueryable;
        }
    }
}
