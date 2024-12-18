using ARWNI2S.Data.Events;
using ARWNI2S.Engine.Common.Data;

namespace ARWNI2S.Engine.Common.Caching
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
