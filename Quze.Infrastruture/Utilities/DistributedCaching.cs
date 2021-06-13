using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Quze.Infrastruture.Utilities
{
    public static class DistributedCaching
    {
        static DistributedCacheEntryOptions DefaultOptions = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromHours(1)
        };

        public async static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options=null, CancellationToken token = default(CancellationToken))
        {
            if (options == null) options = DefaultOptions;
            await distributedCache.SetAsync(key, value.ToByteArray(), options, token);
        }

        public async static Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var result = await distributedCache.GetAsync(key, token);
            return result.FromByteArray<T>();
        }
    }
}