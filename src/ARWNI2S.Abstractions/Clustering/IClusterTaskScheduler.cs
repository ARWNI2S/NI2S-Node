namespace ARWNI2S.Clustering
{
    /// <summary>
    /// Task manager interface
    /// </summary>
    public interface IClusterTaskScheduler
    {
        /// <summary>
        /// Initializes task scheduler
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Starts the task scheduler
        /// </summary>
        public Task StartSchedulerAsync();

        /// <summary>
        /// Stops the task scheduler
        /// </summary>
        public Task StopSchedulerAsync();
    }
}
