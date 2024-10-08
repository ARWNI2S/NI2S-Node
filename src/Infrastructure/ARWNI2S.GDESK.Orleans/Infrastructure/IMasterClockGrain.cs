using Orleans;

namespace ARWNI2S.GDESK.Orleans.Infrastructure
{
    internal interface IMasterClockGrain : IGrainWithIntegerKey
    {
        /// <summary>
        /// Begins the election process.
        /// </summary>
        /// <returns>Async task</returns>
        Task StartElectionAsync();

        /// <summary>
        /// Follower receive heartbeat.
        /// </summary>
        /// <returns>Async task</returns>
        Task ReceiveHeartbeatAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Async task with time result.</returns>
        Task<long> GetCurrentTimeAsync();

        /// <summary>
        /// Called by leader to advance clock.
        /// </summary>
        /// <returns>Async task</returns>
        Task LeaderTickAsync();
    }
}
