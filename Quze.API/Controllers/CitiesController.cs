using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;

namespace Quze.API.Controllers
{
    public class CitiesController : ControllerBase<City, CityVM, CityStore>
    {
        public CitiesController(IMapper mapper, CityStore store) : base(mapper, store)
        {

        }
    }


}