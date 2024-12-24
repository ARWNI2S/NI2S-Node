namespace ARWNI2S.Engine.Events
{
    /// <summary>
    /// Consumer interface
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public interface IEventConsumer<T>
    {
        /// <summary>
        /// Handle event
        /// </summary>
        /// <param name="eventMessage">Event</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task HandleEventAsync(T eventMessage);
    }
}
