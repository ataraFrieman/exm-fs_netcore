using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class ExpertiesController : ControllerBase<Experty, ExpertyVM, ExpertyStore>
    {
        public ExpertiesController(IMapper mapper, ExpertyStore store) : base(mapper, store)
        {

        }
    }


}