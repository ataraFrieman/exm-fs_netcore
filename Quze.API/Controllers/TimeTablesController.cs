using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class TimeTablesController : ControllerBase<TimeTable, TimeTableVM,TimeTableStore>
    {
        public TimeTablesController(IMapper mapper, TimeTableStore store) : base(mapper, store)
        {

        }
    }


}