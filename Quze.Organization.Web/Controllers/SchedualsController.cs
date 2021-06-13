using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.Models;
using System.Threading.Tasks;
using Quze.DAL.Stores;

namespace Quze.Organization.Web.Controllers
{
    public class SchedualsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly QueueStore queueStore;

        public SchedualsController(
            QuzeContext ctx,
            IMapper mapper,
            QueueStore queueStore) : base(ctx, mapper)
        {
            this.queueStore = queueStore;
            this.mapper = mapper;
        }

        //[HttpPost("[action]")]
        //public GetAvailableSlotsResponse GetAvailableSlotsBySp([FromBody] GetAvailableSlotsRequest request)
        //{
        //    return queueStore.GetAvailableSlotsBySp(request);
        //}

        [HttpPost("[action]")]
        public async Task<GetAvailableSlotsResponse> GetAvailableSlotsBySt([FromBody] GetAvailableSlotsRequest request)
        {
            return  queueStore.GetAvailableSlotsBySt(request);
        }

        [HttpPost("[action]")]
        public async Task<GetAvailableSlotsResponse> GetAvailableSlotsToSQ([FromBody] GetAvailableSlotsRequest request)
        {
            return queueStore.GetAvailableSlotsToSQ(request);
        }
    }
}