namespace ARWNI2S.Node.Core.Scheduling
{
    /// <summary>
    /// Interface that should be implemented by each task
    /// </summary>
    public partial interface IClusterJob
    {
        /// <summary>
        /// Executes a task
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task ExecuteAsync();
    }
}