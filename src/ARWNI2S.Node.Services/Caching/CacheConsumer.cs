using ARWNI2S.Node.Caching;
using ARWNI2S.Node.Data.Entities;
using ARWNI2S.Node.Data.Notification;
using ARWNI2S.Node.Engine;
using ARWNI2S.Node.Notification;

namespace ARWNI2S.Node.Services.Caching
{
    /// <summary>
    /// Represents the base entity cache notification consumer
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract partial class CacheConsumer<TEntity> :
        IConsumer<DataEntityCreated<TEntity>>,
        IConsumer<DataEntityUpdated<TEntity>>,
        IConsumer<DataEntityDeleted<TEntity>>
        where TEntity : DataEntity
    {
        #region Fields

        protected readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        protected CacheConsumer()
        {
            _staticCacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Clear cache by notification type
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="notificationType">Entity notification type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task ClearCacheAsync(TEntity entity, NotificationType notificationType)
        {
            await RemoveByPrefixAsync(EntityCacheDefaults<TEntity>.ByIdsPrefix);
            await RemoveByPrefixAsync(EntityCacheDefaults<TEntity>.AllPrefix);

            if (notificationType != NotificationType.Insert)
                await RemoveAsync(EntityCacheDefaults<TEntity>.ByIdCacheKey, entity);

            await ClearCacheAsync(entity);
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual Task ClearCacheAsync(TEntity entity)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Removes items by cache key prefix
        /// </summary>
        /// <param name="prefix">Cache key prefix</param>
        /// <param name="prefixParameters">Parameters to create cache key prefix</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
        {
            await _staticCacheManager.RemoveByPrefixAsync(prefix, prefixParameters);
        }

        /// <summary>
        /// Remove the value with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            await _staticCacheManager.RemoveAsync(cacheKey, cacheKeyParameters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle entity inserted notification
        /// </summary>
        /// <param name="notification">Notification message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task HandleNotificationAsync(DataEntityCreated<TEntity> notification)
        {
            await ClearCacheAsync(notification.Entity, NotificationType.Insert);
        }

        /// <summary>
        /// Handle entity updated notification
        /// </summary>
        /// <param name="notification">Notification message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task HandleNotificationAsync(DataEntityUpdated<TEntity> notification)
        {
            await ClearCacheAsync(notification.Entity, NotificationType.Update);
        }

        /// <summary>
        /// Handle entity deleted notification
        /// </summary>
        /// <param name="notification">Notification message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task HandleNotificationAsync(DataEntityDeleted<TEntity> notification)
        {
            await ClearCacheAsync(notification.Entity, NotificationType.Delete);
        }

        #endregion

        #region Nested

        protected enum NotificationType
        {
            Insert,
            Update,
            Delete
        }

        #endregion
    }
}