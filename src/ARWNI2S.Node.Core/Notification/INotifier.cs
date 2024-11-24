namespace ARWNI2S.Node.Notification
{
    /// <summary>
    /// Represents a notifier
    /// </summary>
    public partial interface INotifier
    {
        /// <summary>
        /// Notify to consumers
        /// </summary>
        /// <typeparam name="TNotification">Type of notification</typeparam>
        /// <param name="notification">Notification object</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task NotifyAsync<TNotification>(TNotification notification);

        /// <summary>
        /// Notify to consumers
        /// </summary>
        /// <typeparam name="TNotification">Type of notification</typeparam>
        /// <param name="notification">Notification object</param>
        void Notify<TNotification>(TNotification notification);
    }
}