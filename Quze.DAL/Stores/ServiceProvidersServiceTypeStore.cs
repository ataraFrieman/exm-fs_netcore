using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.DAL.Stores
{
    public class ServiceProvidersServiceTypeStore : StoreBase<ServiceProvidersServiceType>
    {

        public ServiceProvidersServiceTypeStore(QuzeContext ctx):base(ctx)
        {
                
        }

        public async Task<int> GetAvgDurationAsync(int serviceProviderId, int serviceTypeId)
        {
            var spst = ctx.ServiceProvidersServiceTypes.
                Where(x => x.ServiceProviderId == serviceProviderId && x.ServiceTypeId == serviceTypeId).ToAsyncEnumerable();
            var result = await spst.FirstOrDefault();
            if (result.IsNotNull())
                return result.AvgDuration;
            return 0;
        }

        public List<ServiceProvider> GetSPByServiceTypes( int serviceTypeId)
        {
            var serviceProviders = ctx.ServiceProvidersServiceTypes.
                Where(x => x.ServiceTypeId == serviceTypeId).
                Select(x => x.ServiceProvider).ToList();

            return serviceProviders;
        }

    }
}
