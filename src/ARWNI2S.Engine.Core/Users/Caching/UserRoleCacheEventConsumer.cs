using ARWNI2S.Data.Events;
using ARWNI2S.Engine.Users.Data;

namespace ARWNI2S.Engine.Users.Caching
{
    /// <summary>
    /// Represents a user role cache event consumer
    /// </summary>
    public partial class UserRoleCacheEventConsumer : CacheEventConsumer<UserRole>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(UserRole entity)
        {
            await RemoveByPrefixAsync(UserServicesDefaults.UserRolesBySystemNamePrefix);
            await RemoveByPrefixAsync(UserServicesDefaults.UserUserRolesPrefix);
        }
    }
}
