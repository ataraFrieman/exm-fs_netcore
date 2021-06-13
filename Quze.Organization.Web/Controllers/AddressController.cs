using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quze.DAL;
using Quze.Models.Entities;

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly QuzeContext context;
        public AddressController(QuzeContext ctx/*, IMemoryCache memoryCache, IMapper mapper*/)
        {
            context = ctx;
            //cache = memoryCache;
            // this.mapper = mapper;

        }

        [HttpGet("[action]")]
        public List<City> GetCities()
        {
            var cities = context.Cities
                .Where(q =>
               q.Id == 3000 
               || q.Id == 1137)
               .ToList()
                //  .OrderBy(q => q.Name)
                ;
            // List<AppointmentVM> appointmentsVM = mapper.Map<List<AppointmentVM>>(appointments);

            return cities; 
        }

        [HttpGet("[action]")]
        public List<Street> GetStreets()
        {
            var streets = context.Streets
                .Include(q => q.City)
                .Where(q =>
               ( q.CityId == 3000 || q.CityId == 1137)
                && q.CityId == q.City.Id)
                .ToList()
                //  .OrderBy(q => q.Name)
                ;
            // List<AppointmentVM> appointmentsVM = mapper.Map<List<AppointmentVM>>(appointments);

            return streets;
        }

    }
}
