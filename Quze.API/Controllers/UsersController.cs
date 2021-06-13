using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class UsersController : ControllerBase<User, UserVM,UserStore>
    {
        public UsersController(IMapper mapper, UserStore store) : base(mapper, store)
        {

        }
    }


}