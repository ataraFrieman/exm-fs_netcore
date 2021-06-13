using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Security;
using Quze.Models.Entities;
using Quze.Organization.Web.ViewModels;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quze.Organization.Web.Controllers
{


    [Route("api/[controller]")]
    public class FellowsController : BaseController
    {

        protected readonly FellowStore fellowStore;
        protected readonly QuzeContext context;
        protected readonly IMapper mapper;
        IConfiguration Configuration;

        private Quze.Models.Response<FellowVM> response;


        public FellowsController(
            QuzeContext ctx,
            IConfiguration configuration,
            IMapper mapper,
            FellowStore fellowStore
            ) : base(ctx, mapper)
        {

            this.context = ctx;
            this.mapper = mapper;
            this.Configuration = configuration;
            this.fellowStore = fellowStore;
        }


        [HttpGet("[action]")]
        public Quze.Models.Response<FellowVM> LoadFellowsByOrganizationId()
        {
            response = new Quze.Models.Response<FellowVM>();

            var organizationId = JwtUser.OrganizationId;
            List<Fellow> Fellows = fellowStore.GetFellowsByOrganization(organizationId ?? 0);
            List<FellowVM> FellowsVM = mapper.Map<List<FellowVM>>(Fellows);     
            response.Entities = FellowsVM;
            return response;
        }

        [HttpDelete]
        public Quze.Models.Response<FellowVM> DeleteFellow(Fellow fellow)
        {
            response = new Quze.Models.Response<FellowVM>();
            if (fellow == null)
            {
                response.ResultCode = 204;
                return response;
            }
            else
            {
                fellowStore.DeleteFellow(fellow);
                response.ResultCode = 200;
                return response;
            }
        }

        [HttpPost("[action]")]
        public Quze.Models.Response<FellowVM> UpdateFellow(Fellow fellow)
        {
            response = new Quze.Models.Response<FellowVM>();
            if (fellow == null)
            {
                response.ResultCode = 204;
                return response;
            }
            else
            {
                fellowStore.UpdateFellow(fellow);
                response.ResultCode = 200;
                return response;
            }
        }



    }
}


