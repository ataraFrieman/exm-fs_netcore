using AutoMapper;
using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;

namespace Quze.API.Controllers
{
    public class AppointmentsController : ControllerBase<Appointment, AppointmentVm,AppointmentStore>
    {
        public AppointmentsController(IMapper mapper, AppointmentStore store) : base(mapper, store)
        {

        }
    }


}