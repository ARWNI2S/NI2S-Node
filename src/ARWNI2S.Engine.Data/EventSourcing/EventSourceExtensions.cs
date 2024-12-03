using ARWNI2S.Entities;
using ARWNI2S.EventSourcing;

namespace ARWNI2S.Data.EventSourcing
{
    /// <summary>
    /// Notification extensions
    /// </summary>
    public static class EventSourceExtensions
    {
        /// <summary>
        /// Entity inserted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Notifier</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityInsertedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : IEntity
        {
            await eventPublisher.PublishAsync(new DataEntityCreated<T>(entity));
        }

        /// <summary>
        /// Entity inserted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Notifier</param>
        /// <param name="entity">Entity</param>
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : IEntity
        {
            eventPublisher.Publish(new DataEntityCreated<T>(entity));
        }

        /// <summary>
        /// Entity updated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Notifier</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityUpdatedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : IEntity
        {
            await eventPublisher.PublishAsync(new DataEntityUpdated<T>(entity));
        }

        /// <summary>
        /// Entity updated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Notifier</param>
        /// <param name="entity">Entity</param>
        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : IEntity
        {
            eventPublisher.Publish(new DataEntityUpdated<T>(entity));
        }

        /// <summary>
        /// Entity deleted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Notifier</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityDeletedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : IEntity
        {
            await eventPublisher.PublishAsync(new DataEntityDeleted<T>(entity));
        }

        /// <summary>
        /// Entity deleted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Notifier</param>
        /// <param name="entity">Entity</param>
        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : IEntity
        {
            eventPublisher.Publish(new DataEntityDeleted<T>(entity));
        }
    }
}
