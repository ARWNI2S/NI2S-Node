using ARWNI2S.Core.Data.Localization;
using ARWNI2S.Data.Events;

namespace ARWNI2S.Core.Localization.Caching
{
    /// <summary>
    /// Represents a locale string resource cache event consumer
    /// </summary>
    public partial class LocaleStringResourceCacheEventConsumer : CacheEventConsumer<LocaleStringResource>
    {
        /// <summary>
        /// Clear cache by entity event type
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(LocaleStringResource entity)
        {
            await RemoveAsync(LocalizationServicesDefaults.LocaleStringResourcesAllPublicCacheKey, entity.LanguageId);
            await RemoveAsync(LocalizationServicesDefaults.LocaleStringResourcesAllAdminCacheKey, entity.LanguageId);
            await RemoveAsync(LocalizationServicesDefaults.LocaleStringResourcesAllCacheKey, entity.LanguageId);
            await RemoveByPrefixAsync(LocalizationServicesDefaults.LocaleStringResourcesByNamePrefix, entity.LanguageId);
        }
    }
}
