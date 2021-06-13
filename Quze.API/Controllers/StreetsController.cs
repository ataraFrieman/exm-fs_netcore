using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class StreetsController : ControllerBase<Street, StreetVM,StreetStore>
    {
        public StreetsController(IMapper mapper, StreetStore store) : base(mapper, store)
        {

        }
    }


}