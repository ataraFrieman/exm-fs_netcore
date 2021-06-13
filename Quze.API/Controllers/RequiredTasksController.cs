using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;

namespace Quze.API.Controllers
{
    public class RequiredTasksController : ControllerBase<RequiredTask, RequiredTasksVM,RequiredTaskStore>
    {
        public RequiredTasksController(IMapper mapper, RequiredTaskStore store) : base(mapper, store)
        {

        }
    }


}