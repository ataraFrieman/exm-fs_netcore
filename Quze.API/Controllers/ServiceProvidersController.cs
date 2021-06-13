using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;

namespace Quze.API.Controllers
{
    public class ServiceProvidersController : ControllerBase<ServiceProvider, ServiceProviderVM,ServiceProviderStore>
    {
        public ServiceProvidersController(IMapper mapper, ServiceProviderStore store) : base(mapper, store)
        {

        }



    }


}
