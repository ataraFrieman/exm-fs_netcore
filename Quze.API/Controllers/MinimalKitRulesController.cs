using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class MinimalKitRulesController : ControllerBase<MinimalKitRules, MinimalKitRulesVM,MinimalKitRulesStore>
    {
        public MinimalKitRulesController(IMapper mapper, MinimalKitRulesStore store) : base(mapper, store)
        {

        }
    }


}