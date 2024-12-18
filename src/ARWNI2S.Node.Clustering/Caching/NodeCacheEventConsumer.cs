using ARWNI2S.Clustering.Data;

namespace ARWNI2S.Clustering.Caching
{
    /// <summary>
    /// Represents a node cache event consumer
    /// </summary>
    public partial class NodeCacheEventConsumer : CacheEventConsumer<NI2SNode>
    {
        ///// <summary>
        ///// Clear cache data
        ///// </summary>
        ///// <param name="entity">Entity</param>
        ///// <returns>A task that represents the asynchronous operation</returns>
        //protected override async Task ClearCacheAsync(NI2SNode entity)
        //{
        //    //UNDONE: ADD MORE CACHE CLEANUP HERE AS NEEDED
        //    //await RemoveByPrefixAsync(ni2sEntityCacheDefaults<ShoppingCartItem>.AllPrefix);
        //    //await RemoveByPrefixAsync(LocalizationServicesDefaults.LanguagesByNodePrefix, entity);
        //}
    }
}
