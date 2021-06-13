using AutoMapper;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;

namespace Quze.API.Controllers
{
    public class ServiceProvidersServiceTypesController :ControllerBase<ServiceProvidersServiceType,ServiceProvidersServiceTypeVM,ServiceProvidersServiceTypeStore>
    {
        public ServiceProvidersServiceTypesController(IMapper mapper, ServiceProvidersServiceTypeStore store) : base(mapper, store)
        {

        }
    }


}