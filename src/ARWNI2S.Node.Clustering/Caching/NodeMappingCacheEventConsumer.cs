﻿using ARWNI2S.Clustering.Data;
using ARWNI2S.Clustering.Services;

namespace ARWNI2S.Clustering.Caching
{
    /// <summary>
    /// Represents a node mapping cache event consumer
    /// </summary>
    public partial class NodeMappingCacheEventConsumer : CacheEventConsumer<NodeMapping>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(NodeMapping entity)
        {
            await RemoveAsync(ClusteringServiceDefaults.NodeMappingsCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(ClusteringServiceDefaults.NodeMappingIdsCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(ClusteringServiceDefaults.NodeMappingExistsCacheKey, entity.EntityName);

            //UNDONE: ADD MORE CACHE CLEANUP HERE AS NEEDED
            //if (entity.EntityName.Equals(nameof(Category)))
            //    await RemoveByPrefixAsync(ni2sCatalogDefaults.ChildCategoryIdLookupByNodePrefix, entity.ServerId);
        }
    }
}
