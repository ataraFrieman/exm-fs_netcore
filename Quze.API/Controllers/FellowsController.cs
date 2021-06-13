using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class FellowsController : ControllerBase<Fellow, FellowVM,FellowStore>
    {
        public FellowsController(IMapper mapper, FellowStore store) : base(mapper, store)
        {

        }
    }


}