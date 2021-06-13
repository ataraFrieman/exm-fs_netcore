using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class UserTasksController : ControllerBase<UserTask, UserTaskVM,UserTasksStore>
    {
        public UserTasksController(IMapper mapper, UserTasksStore store) : base(mapper, store)
        {

        }
    }


}