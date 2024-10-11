﻿using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Data.Entities.Directory;
using ARWNI2S.Node.Data.Services.Caching;

namespace ARWNI2S.Node.Data.Services.Directory.Caching
{
    /// <summary>
    /// Represents a country cache event consumer
    /// </summary>
    public partial class CountryCacheEventConsumer : CacheEventConsumer<Country>
    {
        /// <summary>
        /// Clear cache by entity event type
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="entityEventType">Entity event type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Country entity, EntityEventType entityEventType)
        {
            await RemoveByPrefixAsync(EntityCacheDefaults<Country>.Prefix);
        }
    }
}