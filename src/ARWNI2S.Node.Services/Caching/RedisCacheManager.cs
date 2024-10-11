﻿using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Core.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Net;

namespace ARWNI2S.Node.Data.Services.Caching
{
    /// <summary>
    /// Represents a redis distributed cache 
    /// </summary>
    public class RedisCacheManager : DistributedCacheManager
    {
        #region Fields

        private readonly IRedisConnectionWrapper _connectionWrapper;

        #endregion

        #region Ctor

        public RedisCacheManager(AppSettings appSettings,
            IDistributedCache distributedCache,
            IRedisConnectionWrapper connectionWrapper,
            ICacheKeyManager cacheKeyManager)
            : base(appSettings, distributedCache, cacheKeyManager)
        {
            _connectionWrapper = connectionWrapper;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets the list of cache keys prefix
        /// </summary>
        /// <param name="endPoint">Network address</param>
        /// <param name="prefix">String key pattern</param>
        /// <returns>List of cache keys</returns>
        protected virtual async Task<IEnumerable<RedisKey>> GetKeysAsync(EndPoint endPoint, string prefix = null)
        {
            return await (await _connectionWrapper.GetServerAsync(endPoint))
                .KeysAsync((await _connectionWrapper.GetDatabaseAsync()).Database, string.IsNullOrEmpty(prefix) ? null : $"{prefix}*")
                .ToListAsync();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove items by cache key prefix
        /// </summary>
        /// <param name="prefix">Cache key prefix</param>
        /// <param name="prefixParameters">Parameters to create cache key prefix</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
        {
            prefix = PrepareKeyPrefix(prefix, prefixParameters);
            var db = await _connectionWrapper.GetDatabaseAsync();

            foreach (var endPoint in await _connectionWrapper.GetEndPointsAsync())
            {
                var keys = await GetKeysAsync(endPoint, prefix);
                db.KeyDelete(keys.ToArray());
            }

            RemoveByPrefixInstanceData(prefix);
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task ClearAsync()
        {
            await _connectionWrapper.FlushDatabaseAsync();

            ClearInstanceData();
        }

        #endregion
    }
}
