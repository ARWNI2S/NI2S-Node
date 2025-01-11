// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Events
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
