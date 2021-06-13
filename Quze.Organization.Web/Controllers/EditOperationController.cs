
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Quze.Models.Entities;
//using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Quze.DAL;
using Quze.Organization.Web.ViewModels;
using Quze.DAL.Stores;
using Quze.Models;
using Quze.BL.Operations;
using Quze.Models.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;


namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class EditOperationController : BaseController
    {
        private EditOperationLogic editOperationLogic;
        private string sqlDMConnection;

        public EditOperationController(QuzeContext ctx, IMapper mapper, IConfiguration configuration) : base(ctx, mapper)
        {
            editOperationLogic = new EditOperationLogic(ctx, configuration);
            sqlDMConnection = configuration.GetConnectionString("DMConnection");

        }


        [HttpPost("[action]")]
        public async Task<OperationsResponseVM> IsItPossibleToEditAppointmentAsync([FromBody] EditRequest editRequest)
        {
            int OrganizationId = JwtUser.OrganizationId.Value;
            OperationsResponse operationsResponse= await editOperationLogic.EditOperationAsync(editRequest.Entity, editRequest.OperationQueue, OrganizationId, editRequest.OldAppointment, editRequest.State, editRequest.Equipments,sqlDMConnection);
            var rS = mapper.Map<OperationsResponseVM>(operationsResponse);
            return rS;
        }

    }

    public class EditRequest : Request<OperationRecord>
    {
        public OperationsResponse OperationQueue;
        public Appointment OldAppointment;
        public char State;
        public List<Equipment> Equipments { get; set; } = null;
    }
    
}
