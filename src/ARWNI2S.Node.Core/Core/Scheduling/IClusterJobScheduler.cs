namespace ARWNI2S.Node.Core.Scheduling
{
    /// <summary>
    /// Task manager interface
    /// </summary>
    public interface IClusterJobScheduler
    {
        /// <summary>
        /// Initializes task scheduler
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Starts the task scheduler
        /// </summary>
        public void StartScheduler();

        /// <summary>
        /// Stops the task scheduler
        /// </summary>
        public void StopScheduler();
    }
}
