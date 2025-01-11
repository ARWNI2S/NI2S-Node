// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Core;
using ARWNI2S.Events;

namespace ARWNI2S.Engine.Data.Events
{
    /// <summary>
    /// Event publisher extensions
    /// </summary>
    public static class EventPublisherExtensions
    {
        /// <summary>
        /// Entity inserted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityInsertedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : INiisEntity
        {
            await eventPublisher.PublishAsync(new EntityCreatedEvent<T>(entity));
        }

        /// <summary>
        /// Entity inserted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : INiisEntity
        {
            eventPublisher.Publish(new EntityCreatedEvent<T>(entity));
        }

        /// <summary>
        /// Entity updated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityUpdatedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : INiisEntity
        {
            await eventPublisher.PublishAsync(new EntityUpdatedEvent<T>(entity));
        }

        /// <summary>
        /// Entity updated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : INiisEntity
        {
            eventPublisher.Publish(new EntityUpdatedEvent<T>(entity));
        }

        /// <summary>
        /// Entity deleted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityDeletedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : INiisEntity
        {
            await eventPublisher.PublishAsync(new EntityDeletedEvent<T>(entity));
        }

        /// <summary>
        /// Entity deleted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : INiisEntity
        {
            eventPublisher.Publish(new EntityDeletedEvent<T>(entity));
        }
    }
}
