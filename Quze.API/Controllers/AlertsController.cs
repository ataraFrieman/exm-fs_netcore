using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class AlertsController : ControllerBase<Alert, AlertVM,AlertStore>
    {
        public AlertsController(IMapper mapper, AlertStore store) : base(mapper, store)
        {

        }
    }


}