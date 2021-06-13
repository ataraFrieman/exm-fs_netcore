using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quze.DAL;

using Quze.Models.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Quze.Infrastruture.Types;
using Quze.Infrastruture.Extensions;
using Microsoft.EntityFrameworkCore;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models;

namespace Quze.Organization.Web.Controllers
{
    public class ServiceProvidersController : BaseController
    {
        private Quze.Models.Request<ServiceProviderVM> request;
        private Quze.Models.Response<ServiceProviderVM> response;
        protected readonly ServiceProviderStore SPStore;

        public ServiceProvidersController(QuzeContext ctx, IMapper mapper, ServiceProviderStore SPStore) : base(ctx, mapper)
        {
            this.SPStore = SPStore;
        }

        [HttpGet("[action]")]
        public Quze.Models.Response<ServiceProviderVM> GetAll()
        {
            response = new Quze.Models.Response<ServiceProviderVM>();
            if (JwtUser.IsNull() || JwtUser.OrganizationId.IsNull()) return null;
            var OrganizationId = JwtUser.OrganizationId.Value;
            var serviceProvidersList = context.ServiceProviders.OrderBy(sp => sp.FirstName)
                                    .Include(sp => sp.ServiceProvidersBranches)
                .Include(sp => sp.ServiceProvidersServiceTypes)
                .Include(sp => sp.TimeTables)
                .ThenInclude(tt => tt.TimeTableLines)
                .Include(sp => sp.TimeTables)
                .ThenInclude(tt => tt.Branch)
                .Where(x => x.OrganizationId == OrganizationId && (!x.IsDeleted)).ToList();
            if (serviceProvidersList == null)
            {
                response.AddError(Error.WrongId);
                return response;
            }

            var serviceProviderVM = mapper.Map<List<ServiceProviderVM>>(serviceProvidersList);
            response.Entities = serviceProviderVM;
            return response;
        }

        [HttpPost("[action]")]
        public async System.Threading.Tasks.Task<Quze.Models.Response<ServiceProviderVM>> CreateSPAsync(ServiceProvider sp) {
            response = new Quze.Models.Response<ServiceProviderVM>();
            await SPStore.CreateAsync(sp);
            response.Entity = mapper.Map<ServiceProviderVM>(sp);
            return response;
        }

        [HttpDelete]
        public  Response<ServiceProviderVM> DeleteSPAsync(ServiceProvider sp)
        {
            response = new Quze.Models.Response<ServiceProviderVM>();
            SPStore.DeleteSP(sp);
            response.Entity = mapper.Map<ServiceProviderVM>(sp);
            return response;
        }



    }
}