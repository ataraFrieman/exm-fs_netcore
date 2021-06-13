using AutoMapper;
using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models.Entities;

namespace Quze.API.Controllers
{
    public class ServiceTypeController : ControllerBase<ServiceType, ServiceTypeVM, ServiceTypeStore>
    {
        public QuzeContext context { get; set; }
        public ServiceTypeController(IMapper mapper, ServiceTypeStore store,QuzeContext _context) : base(mapper, store)
        {
            this.context = _context;
        }
    }


}