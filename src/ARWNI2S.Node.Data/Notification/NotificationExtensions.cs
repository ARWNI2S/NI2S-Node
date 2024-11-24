using ARWNI2S.Node.Core;
using ARWNI2S.Node.Notification;

namespace ARWNI2S.Node.Data.Notification
{
    /// <summary>
    /// Notification extensions
    /// </summary>
    public static class NotificationExtensions
    {
        /// <summary>
        /// Entity inserted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="notifier">Notifier</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityInsertedAsync<T>(this INotifier notifier, T entity) where T : IEntity
        {
            await notifier.NotifyAsync(new DataEntityCreated<T>(entity));
        }

        /// <summary>
        /// Entity inserted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="notifier">Notifier</param>
        /// <param name="entity">Entity</param>
        public static void EntityInserted<T>(this INotifier notifier, T entity) where T : IEntity
        {
            notifier.Notify(new DataEntityCreated<T>(entity));
        }

        /// <summary>
        /// Entity updated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="notifier">Notifier</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityUpdatedAsync<T>(this INotifier notifier, T entity) where T : IEntity
        {
            await notifier.NotifyAsync(new DataEntityUpdated<T>(entity));
        }

        /// <summary>
        /// Entity updated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="notifier">Notifier</param>
        /// <param name="entity">Entity</param>
        public static void EntityUpdated<T>(this INotifier notifier, T entity) where T : IEntity
        {
            notifier.Notify(new DataEntityUpdated<T>(entity));
        }

        /// <summary>
        /// Entity deleted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="notifier">Notifier</param>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public static async Task EntityDeletedAsync<T>(this INotifier notifier, T entity) where T : IEntity
        {
            await notifier.NotifyAsync(new DataEntityDeleted<T>(entity));
        }

        /// <summary>
        /// Entity deleted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="notifier">Notifier</param>
        /// <param name="entity">Entity</param>
        public static void EntityDeleted<T>(this INotifier notifier, T entity) where T : IEntity
        {
            notifier.Notify(new DataEntityDeleted<T>(entity));
        }
    }
}
