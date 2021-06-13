using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class TimeTableLinesController : ControllerBase<TimeTableLine, TimeTableLineVM,TimeTableLineStore>
    {
        public TimeTableLinesController(IMapper mapper, TimeTableLineStore store) : base(mapper, store)
        {

        }
    }


}