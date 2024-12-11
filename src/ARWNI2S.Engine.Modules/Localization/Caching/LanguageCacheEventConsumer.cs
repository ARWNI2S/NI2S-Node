using ARWNI2S.Core.Data.Localization;
using ARWNI2S.Data.Events;

namespace ARWNI2S.Core.Localization.Caching
{
    /// <summary>
    /// Represents a language cache event consumer
    /// </summary>
    public partial class LanguageCacheEventConsumer : CacheEventConsumer<Language>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Language entity)
        {
            await RemoveAsync(LocalizationServicesDefaults.LocaleStringResourcesAllPublicCacheKey, entity);
            await RemoveAsync(LocalizationServicesDefaults.LocaleStringResourcesAllAdminCacheKey, entity);
            await RemoveAsync(LocalizationServicesDefaults.LocaleStringResourcesAllCacheKey, entity);
            await RemoveByPrefixAsync(LocalizationServicesDefaults.LocaleStringResourcesByNamePrefix, entity);
        }
    }
}