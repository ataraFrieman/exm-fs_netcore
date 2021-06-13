using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Quze.BL.Operations;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Organization.Web.ViewModels;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class StatusOperationController : BaseController
    {
        private StatuseOperationLogic statuseOperationLogic;

        public StatusOperationController(QuzeContext ctx, IMapper mapper) : base(ctx, mapper)
        {
            statuseOperationLogic = new StatuseOperationLogic(ctx);
        }


        [HttpPost("[action]")]
        public OperationsResponseVM UpdadateStatusApointment([FromBody] EditRequest editRequest)
        {
            try
            {
                OperationsResponse operationsResponse = statuseOperationLogic.UpdateStatuseOperation(editRequest.OperationQueue, editRequest.OldAppointment, editRequest.State);
                var rS = mapper.Map<OperationsResponseVM>(operationsResponse);
                return rS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("[action]")]
        public OperationsResponseVM CancelApointment([FromBody] EditRequest editRequest)
        {
            try
            {
                OperationsResponse operationsResponse =statuseOperationLogic.UpdateStatuseOperation(editRequest.OperationQueue, editRequest.OldAppointment, editRequest.State);
                var rS = mapper.Map<OperationsResponseVM>(operationsResponse);
                return rS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("[action]")]
        public bool UpdateDelayApointment([FromBody] Appointment apointment)
        {
            try
            {
                statuseOperationLogic.UpdateDelayOperation(apointment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}