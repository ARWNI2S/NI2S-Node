using System.Net;
using ARWNI2S.Caching;
using ARWNI2S.Collections;
using ARWNI2S.Configuration;
using ARWNI2S.Engine.Caching;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace ARWNI2S.Cluster.Caching
{
    /// <summary>
    /// Represents a redis distributed cache 
    /// </summary>
    public partial class RedisCacheManager : DistributedCacheManager
    {
        #region Fields

        protected readonly IRedisConnectionWrapper _connectionWrapper;

        #endregion

        #region Ctor

        public RedisCacheManager(NI2SSettings settings,
            IDistributedCache distributedCache,
            IRedisConnectionWrapper connectionWrapper,
            ICacheKeyManager cacheKeyManager,
            IConcurrentCollection<object> concurrentCollection)
            : base(settings, distributedCache, cacheKeyManager, concurrentCollection)
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

            var instanceName = _settings.Get<DistributedCacheConfig>().InstanceName ?? string.Empty;

            foreach (var endPoint in await _connectionWrapper.GetEndPointsAsync())
            {
                var keys = await GetKeysAsync(endPoint, instanceName + prefix);
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