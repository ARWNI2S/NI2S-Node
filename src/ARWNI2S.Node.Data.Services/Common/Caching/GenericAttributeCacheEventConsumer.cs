﻿using ARWNI2S.Node.Data.Entities.Common;
using ARWNI2S.Node.Data.Services.Caching;

namespace ARWNI2S.Node.Data.Services.Common.Caching
{
    /// <summary>
    /// Represents a generic attribute cache event consumer
    /// </summary>
    public partial class GenericAttributeCacheEventConsumer : CacheEventConsumer<GenericAttribute>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(GenericAttribute entity)
        {
            await RemoveAsync(CommonServicesDefaults.GenericAttributeCacheKey, entity.EntityId, entity.KeyGroup);
        }
    }
}
