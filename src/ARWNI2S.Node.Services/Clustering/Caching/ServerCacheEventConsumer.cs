﻿using ARWNI2S.Node.Core.Entities.Clustering;
using ARWNI2S.Node.Services.Caching;
using ARWNI2S.Node.Services.Localization;

namespace ARWNI2S.Node.Services.Clustering.Caching
{
    /// <summary>
    /// Represents a node cache event consumer
    /// </summary>
    public partial class ServerCacheEventConsumer : CacheEventConsumer<ClusterNode>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(ClusterNode entity)
        {
            //UNDONE: ADD MORE CACHE CLEANUP HERE AS NEEDED
            //await RemoveByPrefixAsync(ni2sEntityCacheDefaults<ShoppingCartItem>.AllPrefix);
            await RemoveByPrefixAsync(LocalizationServicesDefaults.LanguagesByNodePrefix, entity);
        }
    }
}
