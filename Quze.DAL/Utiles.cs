using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;

namespace Quze.DAL
{
    public static class Utiles
    {
        /// <summary>
        /// filter serviceQueue by specific date 
        /// </summary>
        /// <param name="serviceQueues">a list to filter</param>
        /// <param name="date"> date of service queue </param>
        /// <returns>service queue of date , if dosen't exist return null</returns>
        public static ServiceQueue TryGetServiceQueueOfDate(this IEnumerable<ServiceQueue> serviceQueues, DateTime date)
        {
            var result = serviceQueues.FirstOrDefault(x => x.BeginTime.Date == date.Date);
            return result;
        }

        /// <summary>
        /// gets all service queue of service provider
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="serviceQueues"></param>
        /// <returns>all service queues of given service provider </returns>
        public static IEnumerable<ServiceQueue> ServiceQueuesOfSp(this IEnumerable<ServiceQueue> serviceQueues, ServiceProvider serviceProvider)
        {
            var result = serviceQueues.Where(sq => sq.ServiceProviderId == serviceProvider.Id);
            return result;
        }

        /// <summary>
        /// gets all service provider that support this service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceProviders"> the list of service providers to take from </param>
        /// <param name="serviceProvidersServiceTypes"></param>
        /// <returns></returns>
        public static IEnumerable<ServiceProvider> FilterByServiceType(this IEnumerable<ServiceProvider> serviceProviders, ServiceType serviceType, IEnumerable<ServiceProvidersServiceType> serviceProvidersServiceTypes)
        {

            var serviceProvidersIds = serviceProvidersServiceTypes
                .Where(x => x.ServiceTypeId == serviceType.Id)
                .Select(x => x.Id);

            var serviceProvidersResult =
                serviceProviders
                .Where(x => serviceProvidersIds.Contains(x.Id));

            //from sp in serviceProviders
            //from spid in serviceProvidersIds
            // where sp.Id == spid
            //select sp;

            return serviceProvidersResult;
        }


        /// <summary>
        /// extension method to IEnumerable<ServiceProvider> to filter
        /// </summary>
        /// <param name="serviceProviders"></param>
        /// <param name="organization"></param>
        /// <returns>service providers working in given organization</returns>
        public static IEnumerable<ServiceProvider> FilterByOrganization(this IEnumerable<ServiceProvider> serviceProviders, Organization organization)
        {
            var organizatoinId = organization.Id;
            var result = from sp in serviceProviders
                         where sp.Id == organization.Id
                         select sp;
            return result;
        }


        public static IEnumerable<TimeTable> FilterByBranches(this IEnumerable<TimeTable> timeTables,
            List<Branch> branches)
        {
            if (branches.IsNullOrEmpty())
                return timeTables;

            var result =
                from tt in timeTables
                from branch in branches
                where tt.BranchId == branch.Id
                select tt;

            return result;
        }


        public static IEnumerable<ServiceQueue> FilterByBranches(this IEnumerable<ServiceQueue> serviceQueues,
            IEnumerable<Branch> branches)
        {
            var result = serviceQueues.Where(sq => branches.Any(b => b.Id == sq.BranchId));
            //from sq in serviceQueues
            //from branch in branches
            //where sq.BranchId == branch.Id
            //select sq;

            return result;
        }


        public static IEnumerable<TimeTable> FilterByServiceProviders(this IEnumerable<TimeTable> timeTables,
            List<ServiceProvider> serviceProviders)
        {
            if (serviceProviders.IsNullOrEmpty())
                return timeTables;

            var result =
                from tt in timeTables
                from sp in serviceProviders
                where tt.ServiceProviderId == sp.Id
                select tt;

            //  List<TimeTable> timeTablesResult = result.ToList();
            return result;
        }

        public static IQueryable<ServiceQueue> FilterSqByServiceProviderId(this IQueryable<ServiceQueue> serviceQueues,
            int? serviceProviderId)
        {
            return serviceProviderId.IsNull() ? serviceQueues : serviceQueues.Where(sq => sq.ServiceProviderId == serviceProviderId);
        }

        public static IEnumerable<ServiceQueue> FilterSqByServiceProviders(this IEnumerable<ServiceQueue> serviceQueues,
            IEnumerable<ServiceProvider> serviceProviders)
        {
            if (serviceProviders.IsNullOrEmpty())
                return serviceQueues;

            var result =
                from sq in serviceQueues
                from sp in serviceProviders
                where sq.ServiceProviderId == sp.Id
                select sq;

            //  List<TimeTable> timeTablesResult = result.ToList();
            return result;
        }


        //public static IEnumerable<ServiceQueue> FilterByServiceProviders(this IEnumerable<ServiceQueue> serviceQueues,
        //    IEnumerable<ServiceProvider> serviceProviders)
        //{

        //    var result = serviceQueues.Where(sq => serviceProviders.Any(sp => sp.Id == sq.ServiceProviderId));
        //    //from sq in serviceQueues
        //    //from sp in serviceProviders
        //    //where sq.ServiceProviderId == sp.Id
        //    //select sq;

        //    return result;
        //}

        public static IEnumerable<TimeTable> FilterByServiceType(this IEnumerable<TimeTable> timeTables,
           ServiceType serviceType)
        {
            if (serviceType == null)
                return timeTables;

            var result = timeTables.Where(tt => tt.ServiceTypeId == null || tt.ServiceTypeId.Value == serviceType.Id);
            return result;
        }

        public static IEnumerable<ServiceQueue> FilterByServiceType(this IEnumerable<ServiceQueue> serviceQueues,
            ServiceType serviceType)
        {
            if (serviceType == null)
                return serviceQueues;

            var result = serviceQueues.Where(sq => sq.ServiceTypeId == null || sq.ServiceTypeId == serviceType.Id);
            return result;
        }



        public static bool IsVacation(this ServiceProvider serviceProvider,
            List<TimeTableVacation> timeTableVacations, DateTime date)
        {
            return timeTableVacations.Any(x => x.ServiceProviderId == serviceProvider.Id
                                               && date.Date.IsBetween(x.BeginTime.Date, x.EndTime.Date));
        }


        public static TimeTableException FirstOrDefaultTtException(this TimeTable timeTable,
            DateTime dateToCheck, List<TimeTableException> exceptions)
        {
            return exceptions.FirstOrDefault(x => x.TimeTableId == timeTable.Id && x.DateTime.Date == dateToCheck.Date);
        }


        public static IQueryable<TimeTable> FilterByServiceProviderId(this IQueryable<TimeTable> timeTables,
            int? serviceProviderId)
        {
            return serviceProviderId.IsNull() ? timeTables : timeTables.Where(tt => tt.ServiceProviderId == serviceProviderId);
        }

        public static IQueryable<TimeTable> FilterByCurrentValidity(this IQueryable<TimeTable> timeTables, DateTime? dateTime = null)
        {
            if (dateTime.IsNull()) dateTime = DateTime.Now;
            return timeTables.Where(tt => tt.ValidFromDate <= dateTime && tt.ValidUntilDate >= dateTime);
        }

        public static IQueryable<ServiceQueue> IsBetween(this IQueryable<ServiceQueue> serviceQueues, DateTime beginTime, DateTime endTime)
        {
            return serviceQueues.Where(sq => sq.BeginTime >= beginTime && sq.EndTime <= endTime);
        }


        public static IQueryable<TimeTable> FilterByServiceTypeId(this IQueryable<TimeTable> timeTables,
          QuzeContext ctx, int? serviceTypeId)
        {
            if (serviceTypeId == null)
                return timeTables;

            var children = new ServiceTypeStore(ctx).GetDescendants(serviceTypeId ?? 0).Select(x => x.Id);

            //TODO: 
            var serviceProviderQuery = ctx.ServiceProvidersServiceTypes
                .Where(spst => children.Contains(spst.ServiceTypeId))
                .Select(spst => spst.ServiceProviderId).Distinct();

            return timeTables.Where(tt =>
               (tt.ServiceTypeId == null
          && serviceProviderQuery.Contains(tt.ServiceProviderId)) || children.Contains(tt.ServiceTypeId.Value));
        }


        public static IQueryable<ServiceQueue> FilterSqByServiceTypeId(this IQueryable<ServiceQueue> serviceQueuesQuery, int? serviceTypeId)
        {
            if (serviceTypeId == null)
                return serviceQueuesQuery;

            var newQuery = serviceQueuesQuery.Where(sq => sq.ServiceTypeId == null || sq.ServiceTypeId.Value == serviceTypeId);
            return newQuery;
        }

        public static IQueryable<TimeTable> FilterByBranchId(this IQueryable<TimeTable> timeTables,
            int? branchId)
        {
            return branchId.IsNull() ? timeTables : timeTables.Where(tt => tt.BranchId == branchId);
        }

        public static IQueryable<ServiceQueue> FilterSqByBranchId(this IQueryable<ServiceQueue> serviceQueues,
            int? branchId)
        {
            return branchId.IsNull() ? serviceQueues : serviceQueues.Where(sq => sq.BranchId == branchId);
        }



        public static IQueryable<TimeTable> FilterByOrganizationId(this IQueryable<TimeTable> timeTablesQuery, int organizationId, QuzeContext ctx)
        {
            if (organizationId.IsNull())
                return timeTablesQuery;

            //var branchesIdsQuery =  //new GetEntitiesTemporary(ctx).GetOrganizationBranches(organizationId).Select(x => x.Id);
            timeTablesQuery = timeTablesQuery.Where(tt => ctx.Branches.Select(b => b.Id).Contains(tt.BranchId));
            return timeTablesQuery;
        }

        public static IQueryable<ServiceQueue> FilterByOrganizationId(this IQueryable<ServiceQueue> serviceQueuesQuery
            , int organizationId)
        {
            if (organizationId.IsNull() || organizationId == 0)
                return serviceQueuesQuery;

            serviceQueuesQuery = serviceQueuesQuery.Where(sq => sq.OrganizationId == organizationId);
            return serviceQueuesQuery;
        }













    }
}
