using ARWNI2S.Node.Data.Entities.Localization;
using ARWNI2S.Node.Services.Caching;

namespace ARWNI2S.Node.Services.Localization.Caching
{
    /// <summary>
    /// Represents a locale string resource cache notification consumer
    /// </summary>
    public partial class LocaleStringResourceCacheConsumer : CacheConsumer<LocaleStringResource>
    {
        /// <summary>
        /// Clear cache by notification type
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
