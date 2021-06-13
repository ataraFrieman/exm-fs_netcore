using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quze.Infrastruture.Extensions;
using Quze.Models;

namespace Quze.Organization.Web.Controllers
{
    public class TimeTableController : ControllerBase<TimeTable, TimeTableVM, TimeTableStore>
    {
        public TimeTableController(IMapper mapper, TimeTableStore store) : base(mapper, store)
        {

        }

    }


}