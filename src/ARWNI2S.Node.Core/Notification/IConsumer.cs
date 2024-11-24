namespace ARWNI2S.Node.Notification
{
    /// <summary>
    /// Notification consumer interface
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public interface IConsumer<T>
    {
        /// <summary>
        /// Handle notification
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task HandleNotificationAsync(T notification);
    }
}
