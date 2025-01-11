using ARWNI2S.Cluster.Entities;
using ARWNI2S.Engine.Data.Events;

namespace ARWNI2S.Cluster.Nodes.Caching
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
            await RemoveAsync(NodeDefaults.NodeMappingsCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(NodeDefaults.NodeMappingIdsCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(NodeDefaults.NodeMappingExistsCacheKey, entity.EntityName);

            //if (entity.EntityName.Equals(nameof(Category)))
            //    await RemoveByPrefixAsync(NopCatalogDefaults.ChildCategoryIdLookupByNodePrefix, entity.NodeId);
        }
    }
}