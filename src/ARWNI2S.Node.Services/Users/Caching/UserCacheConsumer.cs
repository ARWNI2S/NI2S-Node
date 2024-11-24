using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Notification;
using ARWNI2S.Node.Services.Caching;

namespace ARWNI2S.Node.Services.Users.Caching
{
    /// <summary>
    /// Represents a user cache notification consumer
    /// </summary>
    public partial class UserCacheConsumer : CacheConsumer<User>, IConsumer<UserPasswordChanged>
    {
        #region Methods

        /// <summary>
        /// Handle password changed notification
        /// </summary>
        /// <param name="notification">Notification message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task HandleNotificationAsync(UserPasswordChanged notification)
        {
            await RemoveAsync(UserServicesDefaults.UserPasswordLifetimeCacheKey, notification.Password.UserId);
        }

        /// <summary>
        /// Clear cache by notification type
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="notificationType">Entity notification type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(User entity, NotificationType notificationType)
        {
            if (notificationType == NotificationType.Delete)
            {
                await RemoveAsync(UserServicesDefaults.UserAddressesCacheKey, entity);
                await RemoveByPrefixAsync(UserServicesDefaults.UserAddressesByUserPrefix, entity);
            }

            await base.ClearCacheAsync(entity, notificationType);
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(User entity)
        {
            await RemoveByPrefixAsync(UserServicesDefaults.UserUserRolesByUserPrefix, entity);
            await RemoveAsync(UserServicesDefaults.UserByGuidCacheKey, entity.UserGuid);

            if (string.IsNullOrEmpty(entity.SystemName))
                return;

            await RemoveAsync(UserServicesDefaults.UserBySystemNameCacheKey, entity.SystemName);
        }

        #endregion
    }
}