﻿using System.Data;
using ARWNI2S.Caching;
using ARWNI2S.Collections;
using ARWNI2S.Configuration;
using ARWNI2S.Engine.Caching;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;

namespace ARWNI2S.Cluster.Caching
{
    /// <summary>
    /// Represents a MsSql server distributed cache 
    /// </summary>
    public partial class MsSqlServerCacheManager : DistributedCacheManager
    {
        #region Fields

        protected readonly DistributedCacheConfig _distributedCacheConfig;

        #endregion

        #region Ctor

        public MsSqlServerCacheManager(NI2SSettings settings,
            IDistributedCache distributedCache,
            ICacheKeyManager cacheKeyManager,
            IConcurrentCollection<object> concurrentCollection)
            : base(settings, distributedCache, cacheKeyManager, concurrentCollection)
        {
            _distributedCacheConfig = settings.Get<DistributedCacheConfig>();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Performs SQL command
        /// </summary>
        /// <param name="command">SQL command</param>
        /// <param name="parameters">Parameters for SQL command</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task PerformActionAsync(SqlCommand command, params SqlParameter[] parameters)
        {
            var conn = new SqlConnection(_distributedCacheConfig.ConnectionString);

            try
            {
                await conn.OpenAsync();
                command.Connection = conn;
                if (parameters.Any())
                    command.Parameters.AddRange(parameters);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                await conn.CloseAsync();
            }
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

            var command =
                new SqlCommand(
                    $"DELETE FROM {_distributedCacheConfig.SchemaName}.{_distributedCacheConfig.TableName} WHERE Id LIKE @Prefix + '%'");

            await PerformActionAsync(command, new SqlParameter("Prefix", SqlDbType.NVarChar) { Value = prefix });

            RemoveByPrefixInstanceData(prefix);
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task ClearAsync()
        {
            var command =
                new SqlCommand(
                    $"TRUNCATE TABLE {_distributedCacheConfig.SchemaName}.{_distributedCacheConfig.TableName}");

            await PerformActionAsync(command);

            ClearInstanceData();
        }

        #endregion
    }
}