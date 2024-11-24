using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Services.Caching;

namespace ARWNI2S.Node.Services.Users.Caching
{
    /// <summary>
    /// Represents a user role cache notification consumer
    /// </summary>
    public partial class UserRoleCacheConsumer : CacheConsumer<UserRole>
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
