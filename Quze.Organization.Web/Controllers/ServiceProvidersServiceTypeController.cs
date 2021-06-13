
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.Models;
using System.Threading.Tasks;
using Quze.DAL.Stores;
using Quze.Models.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Quze.Infrastruture.Types;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class ServiceProvidersServiceTypeController : BaseController
    {
        private Quze.Models.Request<ServiceProviderVM> request;
        private Quze.Models.Response<ServiceProvider> response;
        private readonly ServiceProvidersServiceTypeStore serviceProvidersServiceTypeStore;


        public ServiceProvidersServiceTypeController(QuzeContext ctx, IMapper mapper) : base(ctx, mapper)
        {
                    serviceProvidersServiceTypeStore = new ServiceProvidersServiceTypeStore(ctx);

    }

    [HttpGet("[action]")]
        public List<ServiceProvider> GetSPByServiceType()
        {
            int serviceTypeID = 1;
            var list= serviceProvidersServiceTypeStore.GetSPByServiceTypes(serviceTypeID);
            return list ;
        }

        [HttpGet("[action]")]
        public List<ServiceProvider> GetSPByServiceType(int serviceTypeID)
        {
            var list = serviceProvidersServiceTypeStore.GetSPByServiceTypes(serviceTypeID);
            return list;
        }


    }
}