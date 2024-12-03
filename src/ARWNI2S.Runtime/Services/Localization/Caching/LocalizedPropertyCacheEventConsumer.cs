using ARWNI2S.Data.Entities.Localization;
using ARWNI2S.Services.Caching;

namespace ARWNI2S.Services.Localization.Caching
{
    /// <summary>
    /// Represents a localized property cache event consumer
    /// </summary>
    public partial class LocalizedPropertyCacheEventConsumer : DataCacheEventConsumer<LocalizedProperty>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(LocalizedProperty entity)
        {
            await RemoveAsync(LocalizationServicesDefaults.LocalizedPropertyCacheKey, entity.LanguageId, entity.EntityId, entity.LocaleKeyGroup, entity.LocaleKey);
            await RemoveAsync(LocalizationServicesDefaults.LocalizedPropertiesCacheKey, entity.EntityId, entity.LocaleKeyGroup, entity.LocaleKey);
            await RemoveAsync(LocalizationServicesDefaults.LocalizedPropertyLookupCacheKey, entity.LanguageId);
        }
    }
}
