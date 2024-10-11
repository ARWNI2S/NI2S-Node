using ARWNI2S.Node.Data.Entities.Common;
using ARWNI2S.Node.Data.Services.Caching;
using ARWNI2S.Node.Data.Services.Users;

namespace ARWNI2S.Node.Data.Services.Common.Caching
{
    /// <summary>
    /// Represents a address cache event consumer
    /// </summary>
    public partial class AddressCacheEventConsumer : CacheEventConsumer<Address>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Address entity)
        {
            await RemoveByPrefixAsync(UserServicesDefaults.UserAddressesPrefix);
        }
    }
}
