using ARWNI2S.Node.Data.Entities.Clustering;
using ARWNI2S.Node.Services.Caching;

namespace ARWNI2S.Node.Services.Clustering.Caching
{
    /// <summary>
    /// Represents a node mapping cache notification consumer
    /// </summary>
    public partial class NodeMappingCacheConsumer : CacheConsumer<NodeMapping>
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
