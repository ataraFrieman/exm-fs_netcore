using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class TimeTableVacationsController : ControllerBase<TimeTableVacation, TimeTableVacationVM,TimeTableVacationStore>
    {
        public TimeTableVacationsController(IMapper mapper, TimeTableVacationStore store) : base(mapper, store)
        {

        }
    }


}