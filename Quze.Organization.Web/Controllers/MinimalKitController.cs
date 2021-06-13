using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.Models.Entities;
using Quze.BL;
using Quze.Organization.Web.ViewModels;
using Quze.Models;
using Quze.Infrastruture.Extensions;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinimalKitController : BaseController
    {
        private readonly IConfiguration configuration;
        private MinimalKitLogic minimalKitLogic;

        public MinimalKitController(QuzeContext ctx, IMapper mapper, IConfiguration configuration) : base(ctx, mapper)
        {

            minimalKitLogic = new MinimalKitLogic(context, configuration);
            this.configuration = configuration;
        }
        public class MKDescription
        {
            public string description { get; set; }
            public bool isRequired { get; set; }
            public int appointmentId { get; set; }
        }

        //public class MinimalKitRequest
        //{
        //    public int AppointmentId { get; set; }
        //    public List<AppointmentDocument> Docs { get; set; }
        //    public List<AppointmentTask> Tasks { get; set; }
        //    public List<AppointmentTest> Tests { get; set; }
        //}


        //public class MinimalKitResponse : Response<int>
        //{
        //    public int AppointmentId { get; set; }
        //    public List<AppointmentDocument> Docs { get; set; } = null;
        //    public List<AppointmentTask> Tasks { get; set; } = null;
        //    public List<AppointmentTest> Tests { get; set; } = null;
        //}


        //public class MinimalKitResponseVM : Response<int>
        //{
        //    public int AppointmentId { get; set; }
        //    public List<AppointmentDocVM> Docs { get; set; } = null;
        //    public List<AppointmentTaskVM> Tasks { get; set; } = null;
        //    public List<AppointmentTestVM> Tests { get; set; } = null;
        //}


        // GET: api/MinimalKit
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MinimalKit/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MinimalKit
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        [HttpPost("[action]")]
        public AppointmentDocument AddDocument([FromBody] MKDescription description)//Add document to appointment.
        {
            return minimalKitLogic.AddDocumentToAppointment(description.description, description.isRequired, description.appointmentId);
        }
        [HttpPost("[action]")]
        public AppointmentTask AddTask([FromBody] MKDescription description)//Add task to appointment.
        {
            return minimalKitLogic.AddTaskToAppointment(description.description, description.isRequired, description.appointmentId);
        }
        // PUT: api/MinimalKit/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("[action]")]
        public MinimalKitVM SaveMinimalKit(MinimalKit request)
        {
            try
            {
                var minimalKitResponseVM = new MinimalKitVM();
                var minimalKit = new MinimalKit();

                if (request.AppointmentId == 0)
                {
                    minimalKitResponseVM.AddError(1000, "AppointmentId <= 0");
                }

                if (request.IsNull())
                {
                    minimalKitResponseVM.AddError(1500, "Request is NULL!!");
                }

                minimalKit = minimalKitLogic.SaveMinimalKit(request);
                if (minimalKit.IsNull())
                {
                    minimalKitResponseVM.AddError(3000, "Saving MK didn't successed");
                    return minimalKitResponseVM;
                }
                minimalKit.mkStauts = minimalKitLogic.UpdateMkStatus(request, request.operationId);
                minimalKit.AppointmentId = request.AppointmentId;

                minimalKitResponseVM = mapper.Map<MinimalKitVM>(minimalKit);
                return minimalKitResponseVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
