
using System.Collections.Generic;
using System.Linq;
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
    public class EquipmentController : BaseController
    {

        protected readonly QuzeContext context;
        protected readonly IMapper mapper;
        IConfiguration Configuration;

        private Quze.Models.Response<EquipmentVM> response;
        private EquipmentStore equipmentStore;

        public EquipmentController(
            QuzeContext ctx,
            IConfiguration configuration,
            IMapper mapper
            ) : base(ctx, mapper)
        {

            this.context = ctx;
            this.mapper = mapper;
            this.Configuration = configuration;
            equipmentStore = new EquipmentStore(ctx);
        }


        [HttpGet("[action]")]
        public Quze.Models.Response<EquipmentVM> GetEquipmentsByOrganizationId()
        {
            response = new Quze.Models.Response<EquipmentVM>();
            var organizationId = JwtUser.OrganizationId;
            List<Equipment> equipments = equipmentStore.GetEquipmentsByOrganization(organizationId ?? 0);
            List<EquipmentVM > equipmentVM = mapper.Map<List<EquipmentVM>>(equipments);
            response.Entities = equipmentVM;
            return response;
        }

        //[HttpDelete]
        //public Quze.Models.Response<FellowVM> DeleteFellow(Fellow fellow)
        //{
        //    response = new Quze.Models.Response<FellowVM>();
        //    if (fellow == null)
        //    {
        //        response.ResultCode = 204;
        //        return response;
        //    }
        //    else
        //    {
        //        fellowStore.DeleteFellow(fellow);
        //        response.ResultCode = 200;
        //        return response;
        //    }
        //}

        //[HttpPost("[action]")]
        //public Quze.Models.Response<FellowVM> UpdateFellow(Fellow fellow)
        //{
        //    response = new Quze.Models.Response<FellowVM>();
        //    if (fellow == null)
        //    {
        //        response.ResultCode = 204;
        //        return response;
        //    }
        //    else
        //    {
        //        fellowStore.UpdateFellow(fellow);
        //        response.ResultCode = 200;
        //        return response;
        //    }
        //}



    }
}


