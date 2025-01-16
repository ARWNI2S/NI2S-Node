﻿using ARWNI2S.Caching;
using ARWNI2S.Collections;
using ARWNI2S.Configuration;
using ARWNI2S.Engine.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace ARWNI2S.Cluster.Caching
{
    public partial class MemoryDistributedCacheManager : DistributedCacheManager
    {
        #region Ctor

        public MemoryDistributedCacheManager(NI2SSettings settings,
            IDistributedCache distributedCache,
            ICacheKeyManager cacheKeyManager,
            IConcurrentCollection<object> concurrentCollection)
            : base(settings, distributedCache, cacheKeyManager, concurrentCollection)
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