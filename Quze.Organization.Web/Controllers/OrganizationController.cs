using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Security;
using Quze.Models;
using Quze.Models.Entities;
using Quze.Organization.Web.ViewModels;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class OrganizationController : BaseController
    {
        protected readonly QueueStore queueStore;
        protected readonly OrganizationStore organizationStore;
        protected readonly QuzeContext context;
        protected readonly IMapper mapper;
        IConfiguration Configuration;
        IUserService userService;
        IUserStore<User> userStore;

        public OrganizationController(
            QuzeContext ctx,
            IConfiguration configuration,
            IMapper mapper,
            OrganizationStore organizationStore,
            QueueStore queueStore,
            IUserService userService,
           IUserStore<User> userStore
            ) : base(ctx, mapper)
        {
            this.queueStore = queueStore;
            this.organizationStore = organizationStore;
            this.context = ctx;
            this.mapper = mapper;
            this.Configuration = configuration;
            this.userService = userService;
            this.userStore = userStore;
        }


        [HttpGet()]
        public async Task<Response<Quze.Models.Entities.Organization>> LoadOrganizationDatails(int organizationId)
        {
            var response = new Response<Quze.Models.Entities.Organization>();
            if (organizationId == 0)
            {
                response.AddError(404, "wrong ornganization id");
                return response;
            }
            var organization = await organizationStore.GetByIdAsync(organizationId);


            response.Entity = organization;
            return response;
        }

        [HttpPost("[action]")]
        public ServiceProvider CreateServiceProvider([FromBody]ServiceProvider serviceProvider)
        {
            foreach (var item in serviceProvider.ServiceProvidersServiceTypes)
            {
                item.ServiceProviderId = serviceProvider.Id;
                context.Entry(item.ServiceType).State = EntityState.Unchanged;
            }

            context.ServiceProviders.AddRange(serviceProvider);

            context.SaveChanges();

            foreach (var item in serviceProvider.ServiceProvidersServiceTypes)
            {
                item.ServiceProvider = null;
                item.ServiceType.ServiceProvidersServiceTypes = null;
            }

            return serviceProvider;
        }

        [HttpPost("[action]")]
        public int CreateFellow([FromBody]Fellow fellow)
        {
            context.Fellows.AddRange(fellow);
            context.SaveChanges();

            return fellow.Id;
        }

        [HttpGet("[action]")]
        public List<ServiceQueueVM> GetServiceQueue(int serviceProviderId, int branchId, DateTime dt, DateTime endDate, bool timeTable)
        {
            //dt = new DateTime(2018, 12, 30);
            Branch branch = new Branch();
            var b = context.Branches
                .FirstOrDefault(o => o.Id == branchId);

            branch = b;

            var s = context.ServiceProviders
                        .FirstOrDefault(o => o.Id == serviceProviderId);


            var serviceProvider = s;

            var serviceQueues = queueStore.GetCalendarData(serviceProviderId, branchId, dt, 0, branch, serviceProvider, endDate);
            var serviceQueuesVm = mapper.Map<List<ServiceQueueVM>>(serviceQueues);

            return serviceQueuesVm;

        }

        [HttpGet("[action]")]
        public IEnumerable<TimeTable> SchdulePlanningData(int serviceProviderId, int branchId, DateTime dt, DateTime endDate, bool timeTable)
        {
            //dt = new DateTime(2018, 12, 30);
            var b = context.Branches
                .FirstOrDefault(o => o.Id == branchId);
            var branch = b;

            var s = context.ServiceProviders
                .FirstOrDefault(o => o.Id == serviceProviderId);

            var serviceProvider = s;

            var timeTables = queueStore.SchdulePlanningData(serviceProviderId, branchId, dt, branch, serviceProvider, endDate, timeTable);
            //List<ServiceQueueVM> serviceQueuesVM = mapper.Map<List<ServiceQueueVM>>(serviceQueues);

            return timeTables;

        }

        [HttpPost("[action]")]
        public TimeTableVM CreateTimeTableLine([FromBody]TimeTable newTt)
        {
            var timeTables = queueStore.CreateTimeTableLine(newTt);
            var timeTableVm = mapper.Map<TimeTableVM>(timeTables);

            return timeTableVm;
        }

        [HttpGet("[action]")]
        public async Task<IList<GlobalSearchResultRecord>> GlobalSearch(string term)
        {
            if (JwtUser.IsNull() || JwtUser.OrganizationId.IsNull()) return null;

            using (GlobalSearch globalSearch = new GlobalSearch(Configuration.GetConnectionString("DefaultConnection")))
            {
                await globalSearch.Search(JwtUser.OrganizationId.Value, term);
                return globalSearch.Results.ToList();
            }
        }

        [HttpGet("[action]")]
        public async Task<IList<GlobalSearchResultRecord>> FellowSearch(string term)
        {
            if (JwtUser.IsNull() || JwtUser.OrganizationId.IsNull()) return null;
            List<GlobalSearchResultRecord> resultsList = new List<GlobalSearchResultRecord>();
            List<Fellow> fellowsList = context.Fellows.Where(f => f.OrganizationId==JwtUser.OrganizationId&&( f.FullName.ToLower().Contains(term.ToLower()) || f.IdentityNumber.ToLower().Contains(term.ToLower()))).ToList();
            if (fellowsList != null)
                for (int i = 0; i < fellowsList.Count; i++)
                {
                    Fellow fellow = fellowsList[i];
                    GlobalSearchResultRecord res = new GlobalSearchResultRecord() { Id = fellow.Id.ToString(), Name = fellow.FullName, SearchResultType = SearchResultType.Fellow };
                    resultsList.Add(res);
                }
            return resultsList;
            //using (GlobalSearch globalSearch = new GlobalSearch(Configuration.GetConnectionString("DefaultConnection")))
            //{
            //    await globalSearch.FellowsSearch(term,JwtUser.OrganizationId);
            //    return globalSearch.Results.ToList();
            //}
        }

        [HttpGet("[action]")]
        public List<ServiceQueueVM> GetCurrentQuze(int branchId)
        {
            var serviceQueues = queueStore.GetCurrentQuze(branchId);
            var serviceQueuesVm = mapper.Map<List<ServiceQueueVM>>(serviceQueues);

            return serviceQueuesVm;

        }
    }
}