using AutoMapper;
using Quze.API.ViewModels;
using Quze.DAL.Stores;
using Quze.Models.Entities;

namespace Quze.API.Controllers
{
    public class ServiceQueuesController : ControllerBase<ServiceQueue, ServiceQueueVm,ServiceQueueStore>
    {
        public ServiceQueuesController(IMapper mapper, ServiceQueueStore store) : base(mapper, store)
        {

        }
    }


}