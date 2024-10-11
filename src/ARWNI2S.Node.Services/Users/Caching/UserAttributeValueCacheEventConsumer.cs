using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Data.Services.Caching;

namespace ARWNI2S.Node.Data.Services.Users.Caching
{
    /// <summary>
    /// Represents a user attribute value cache event consumer
    /// </summary>
    public partial class UserAttributeValueCacheEventConsumer : CacheEventConsumer<UserAttributeValue>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(UserAttributeValue entity)
        {
            await RemoveAsync(UserServicesDefaults.UserAttributeValuesByAttributeCacheKey, entity.UserAttributeId);
        }
    }
}