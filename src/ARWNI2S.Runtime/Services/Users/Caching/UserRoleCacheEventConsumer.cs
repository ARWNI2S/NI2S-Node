using ARWNI2S.Data.Entities.Users;
using ARWNI2S.Services.Caching;

namespace ARWNI2S.Services.Users.Caching
{
    /// <summary>
    /// Represents a user role cache event consumer
    /// </summary>
    public partial class UserRoleCacheEventConsumer : DataCacheEventConsumer<UserRole>
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
