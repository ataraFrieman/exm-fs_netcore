using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class TimeTableExceptionsController : ControllerBase<TimeTableException, TimeTableExceptionVM,TimeTableExceptionStore>
    {
        public TimeTableExceptionsController(IMapper mapper, TimeTableExceptionStore store) : base(mapper, store)
        {

        }
    }


}