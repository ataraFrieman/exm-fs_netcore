using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Quze.BL.Operations;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using static Quze.Organization.Web.Controllers.OperationsController;

namespace Quze.Organization.Web.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class NewInlayController : BaseController
    {
        const int LifeSavingPriority = 1;
        OperationConflictLogic operationConflictLogic;
        EditOperationLogic editOperationLogic;
        NewInllayLogic newInllayLogic;

        public NewInlayController(QuzeContext ctx,IMapper mapper, IMemoryCache _cache, IConfiguration configuration) : base(ctx, mapper)
        {
            operationConflictLogic = new OperationConflictLogic(ctx);
            editOperationLogic = new EditOperationLogic(ctx);
            newInllayLogic = new NewInllayLogic(ctx, configuration);

        }
        // GET: api/NewInlay
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/NewInlay/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/NewInlay
        [HttpPost]
        public async Task<OperationsResponse> PostAsync([FromBody] ReschedualRequest request)
        {
            OperationsResponse response =await newInllayLogic.RescheduleAsync(request.operationQueue,request.BeginTime,request.Equipments);
            return response;
        }

        // PUT: api/NewInlay/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}