
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quze.BL.Operations;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Security;
using Quze.Models.Entities;
using Quze.Organization.Web.Pages.ViewModels;
using Quze.Organization.Web.ViewModels;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class ReadyOperationController : BaseController
    {
        protected readonly QuzeContext context;
        protected readonly IMapper mapper;
        private EditOperationLogic editOperationLogic;


        private Quze.Models.Response<AppointmentOppVM> response;


        public ReadyOperationController( QuzeContext ctx,IMapper mapper):base(ctx, mapper)
        {
            this.context = ctx;
            this.mapper = mapper;
            editOperationLogic = new EditOperationLogic(ctx);
        }

        [HttpPost("[action]")]
        public ResponseReadyEqpOperationVM UpdateRadyEquipmentOperation(ReadyEqpOperation readyEqpOperation)
        {
            ResponseReadyEqpOperation responseReadyEqpOperation = editOperationLogic.UpdateRadyEquipmentOperation(readyEqpOperation);
            ResponseReadyEqpOperationVM responseReadyEqpOperationVM = mapper.Map<ResponseReadyEqpOperationVM>(responseReadyEqpOperation);
            return responseReadyEqpOperationVM;
        }

        [HttpPost("[action]")]
        //save every appointment equipments that he need
        public List<EquipmentAppointmentRequestVM> EquipmentRequest(ReadyEqpOperation readyEqpOperation)
        {
            //NonEnabledEquipmentList
            var response=editOperationLogic.AddEquipmentAppointmentRequest(readyEqpOperation);
            List<EquipmentAppointmentRequestVM> equipmentAppointmentRequestVM = mapper.Map<List<EquipmentAppointmentRequestVM>>(response);
            return equipmentAppointmentRequestVM;
        }
    }
}


