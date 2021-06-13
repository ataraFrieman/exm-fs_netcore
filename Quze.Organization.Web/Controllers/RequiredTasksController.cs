using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quze.Infrastruture.Extensions;
using Quze.Models;
using System.Linq;

namespace Quze.Organization.Web.Controllers
{
    public class RequiredTasksController : ControllerBase<RequiredTask, RequiredTaskVM, RequiredTaskStore>
    {
        public RequiredTasksController(IMapper mapper, RequiredTaskStore store) : base(mapper, store)
        {

        }

        // GET
        [HttpGet("[action]")]
        public async Task<Response<RequiredTaskVM>> GetTasksBySTAsync(int id)
        {
            var entity = store.GetTasksByST(id);
            var entityVM = new List<RequiredTaskVM>();
            foreach (var e in entity)
            {
                entityVM.Add(mapper.Map<RequiredTaskVM>(e));
            }

            var response = new Response<RequiredTaskVM> { Entities = entityVM };
            return response;
        }

    }


}