using ARWNI2S.Data.Entities.Configuration;
using ARWNI2S.Services.Caching;

namespace ARWNI2S.Services.Configuration.Caching
{
    /// <summary>
    /// Represents a setting cache event consumer
    /// </summary>
    public partial class SettingCacheEventConsumer : DataCacheEventConsumer<Setting>
    {
        /// <summary>
        /// Clear cache by entity event type
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="entityEventType">Entity event type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override Task ClearCacheAsync(Setting entity, EntityEventType entityEventType)
        {
            //clear setting cache in SettingService
            return Task.CompletedTask;
        }
    }
}