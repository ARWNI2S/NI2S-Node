namespace ARWNI2S.EventSourcing
{
    /// <summary>
    /// Event consumer interface
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public interface IConsumer<T>
    {
        /// <summary>
        /// Handle event
        /// </summary>
        /// <param name="event">Event</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task HandleEventAsync(T @event);
    }
}
