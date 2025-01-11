using ARWNI2S.Cluster.Entities;
using ARWNI2S.Engine.Data.Events;

namespace ARWNI2S.Cluster.Nodes.Caching
{
    /// <summary>
    /// Represents a node cache event consumer
    /// </summary>
    public partial class NodeCacheEventConsumer : CacheEventConsumer<Node>
    {
        ///// <summary>
        ///// Clear cache data
        ///// </summary>
        ///// <param name="entity">Entity</param>
        ///// <returns>A task that represents the asynchronous operation</returns>
        //protected override async Task ClearCacheAsync(Node entity)
        //{
        //    await RemoveByPrefixAsync(EntityCacheDefaults<ShoppingCartItem>.AllPrefix);
        //    await RemoveByPrefixAsync(NopLocalizationDefaults.LanguagesByNodePrefix, entity);
        //}
    }
}