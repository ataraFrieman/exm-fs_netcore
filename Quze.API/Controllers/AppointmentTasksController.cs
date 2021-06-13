using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class AppointmentTasksController : ControllerBase<AppointmentTask, AppointmentTaskVM,AppointmentTaskStore >
    {
        public AppointmentTasksController(IMapper mapper, AppointmentTaskStore store) : base(mapper, store)
        {

        }
    }


}