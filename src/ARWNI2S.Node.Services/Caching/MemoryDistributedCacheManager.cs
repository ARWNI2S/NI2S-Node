using ARWNI2S.Infrastructure.Collections;
using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Core.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace ARWNI2S.Node.Services.Caching
{
    public class MemoryDistributedCacheManager : DistributedCacheManager
    {
        #region Ctor

        public MemoryDistributedCacheManager(NI2SSettings ni2sSettings,
            IDistributedCache distributedCache,
            ICacheKeyManager cacheKeyManager,
            IConcurrentCollection<object> concurrentCollection)
            : base(ni2sSettings, distributedCache, cacheKeyManager, concurrentCollection)
        {
        }

        #endregion

        /// <summary>
        /// Clear all cache data
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task ClearAsync()
        {
            foreach (var key in _localKeyManager.Keys)
                await RemoveAsync(key, false);

            ClearInstanceData();
        }

        /// <summary>
        /// Remove items by cache key prefix
        /// </summary>
        /// <param name="prefix">Cache key prefix</param>
        /// <param name="prefixParameters">Parameters to create cache key prefix</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
        {
            var keyPrefix = PrepareKeyPrefix(prefix, prefixParameters);

            foreach (var key in RemoveByPrefixInstanceData(keyPrefix))
                await RemoveAsync(key, false);
        }
    }
}
