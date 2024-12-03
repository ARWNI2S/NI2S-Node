namespace ARWNI2S.EventSourcing
{
    /// <summary>
    /// Represents a event publisher
    /// </summary>
    public partial interface IEventPublisher
    {
        /// <summary>
        /// Notify to consumers
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="event">Event object</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task PublishAsync<TEvent>(TEvent @event);

        /// <summary>
        /// Notify to consumers
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="event">Event object</param>
        void Publish<TEvent>(TEvent @event);
    }
}