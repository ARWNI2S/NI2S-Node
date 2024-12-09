using ARWNI2S.Node.Core.Entities.Security;
using ARWNI2S.Node.Services.Caching;

namespace ARWNI2S.Node.Services.Security.Caching
{
    /// <summary>
    /// Represents a ACL record cache event consumer
    /// </summary>
    public partial class AclRecordCacheEventConsumer : CacheEventConsumer<AclRecord>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(AclRecord entity)
        {
            await RemoveAsync(SecurityServicesDefaults.AclRecordCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(SecurityServicesDefaults.EntityAclRecordExistsCacheKey, entity.EntityName);
        }
    }
}
