//using TheCorporateWars.Server.Caching;
//using TheCorporateWars.Server.Domain.Orders;
using ARWNI2S.Node.Data.Entities.Clustering;
using ARWNI2S.Node.Data.Services.Caching;
using ARWNI2S.Node.Data.Services.Localization;

namespace ARWNI2S.Node.Data.Services.Clustering.Caching
{
    /// <summary>
    /// Represents a node cache event consumer
    /// </summary>
    public partial class ServerCacheEventConsumer : CacheEventConsumer<BladeServer>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(BladeServer entity)
        {
            //UNDONE: ADD MORE CACHE CLEANUP HERE AS NEEDED
            //await RemoveByPrefixAsync(ni2sEntityCacheDefaults<ShoppingCartItem>.AllPrefix);
            await RemoveByPrefixAsync(LocalizationServicesDefaults.LanguagesByNodePrefix, entity);
        }
    }
}
