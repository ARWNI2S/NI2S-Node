﻿using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Core.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace ARWNI2S.Node.Data.Services.Caching
{
    public class MemoryDistributedCacheManager : DistributedCacheManager
    {
        #region Ctor

        public MemoryDistributedCacheManager(AppSettings appSettings,
            IDistributedCache distributedCache,
            ICacheKeyManager cacheKeyManager)
            : base(appSettings, distributedCache, cacheKeyManager)
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
