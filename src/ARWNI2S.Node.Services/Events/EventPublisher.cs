﻿using ARWNI2S.Node.Core.Events;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Services.Logging;

namespace ARWNI2S.Node.Services.Events
{
    /// <summary>
    /// Represents the event publisher implementation
    /// </summary>
    public partial class EventPublisher : INodeEventPublisher
    {
        #region Methods

        /// <summary>
        /// Publish event to consumers
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="event">Event object</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task PublishAsync<TEvent>(TEvent @event)
        {
            //get all event consumers
            var consumers = NodeEngineContext.Current.ResolveAll<IConsumer<TEvent>>().ToList();
            foreach (var consumer in consumers)
            {
                try
                {
                    //try to handle published event
                    await consumer.HandleEventAsync(@event);
                }
                catch (Exception exception)
                {
                    //log error, we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                    try
                    {
                        var logger = NodeEngineContext.Current.Resolve<ILogService>();
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
        /// Publish event to consumers
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="event">Event object</param>
        public virtual void Publish<TEvent>(TEvent @event)
        {
            //get all event consumers
            var consumers = NodeEngineContext.Current.ResolveAll<IConsumer<TEvent>>().ToList();

            foreach (var consumer in consumers)
                try
                {
                    //try to handle published event
                    consumer.HandleEventAsync(@event).Wait();
                }
                catch (Exception exception)
                {
                    //log error, we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                    try
                    {
                        var logger = NodeEngineContext.Current.Resolve<ILogService>();
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