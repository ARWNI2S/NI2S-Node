using ARWNI2S.Data.Entities.Users;
using ARWNI2S.Services.Caching;

namespace ARWNI2S.Services.Users.Caching
{
    /// <summary>
    /// Represents a user user role mapping cache event consumer
    /// </summary>
    public partial class UserUserRoleMappingCacheEventConsumer : DataCacheEventConsumer<UserUserRoleMapping>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(UserUserRoleMapping entity)
        {
            await RemoveByPrefixAsync(UserServicesDefaults.UserUserRolesPrefix);
        }
    }
}