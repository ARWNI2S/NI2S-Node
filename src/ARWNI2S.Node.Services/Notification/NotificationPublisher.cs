using ARWNI2S.Node.Engine;
using ARWNI2S.Node.Notification;
using ARWNI2S.Node.Services.Logging;

namespace ARWNI2S.Node.Services.Notification
{
    /// <summary>
    /// Represents the notifier implementation
    /// </summary>
    public partial class NotificationPublisher : INotifier
    {
        #region Methods

        /// <summary>
        /// Notify to consumers
        /// </summary>
        /// <typeparam name="TNotification">Type of notification</typeparam>
        /// <param name="notification">Notification object</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task NotifyAsync<TNotification>(TNotification notification)
        {
            //get all notification consumers
            var consumers = EngineContext.Current.ResolveAll<IConsumer<TNotification>>().ToList();
            foreach (var consumer in consumers)
            {
                try
                {
                    //try to handle notification
                    await consumer.HandleNotificationAsync(notification);
                }
                catch (Exception exception)
                {
                    //log error, we put in to nested try-catch to prnotification possible cyclic (if some error occurs)
                    try
                    {
                        var logger = EngineContext.Current.Resolve<ILogService>();
                        if (logger == null)
                            return;

                        await logger.ErrorAsync(exception.Message, exception);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        /// <summary>
        /// Notify to consumers
        /// </summary>
        /// <typeparam name="TNotification">Type of notification</typeparam>
        /// <param name="notification">Notification object</param>
        public virtual void Notify<TNotification>(TNotification notification)
        {
            //get all notification consumers
            var consumers = EngineContext.Current.ResolveAll<IConsumer<TNotification>>().ToList();

            foreach (var consumer in consumers)
                try
                {
                    //try to handle notification
                    consumer.HandleNotificationAsync(notification).Wait();
                }
                catch (Exception exception)
                {
                    //log error, we put in to nested try-catch to prnotification possible cyclic (if some error occurs)
                    try
                    {
                        var logger = EngineContext.Current.Resolve<ILogService>();
                        if (logger == null)
                            return;

                        logger.Error(exception.Message, exception);
                    }
                    catch
                    {
                        // ignored
                    }
                }
        }

        #endregion
    }
}