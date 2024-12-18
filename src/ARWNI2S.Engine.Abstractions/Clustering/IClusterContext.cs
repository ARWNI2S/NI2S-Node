namespace ARWNI2S.Engine.Clustering
{
    /// <summary>
    /// Cluster context
    /// </summary>
    public interface IClusterContext
    {
        /// <summary>
        /// Gets the current node
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<INiisNode> GetCurrentNodeAsync();

        /// <summary>
        /// Gets the current node
        /// </summary>
        INiisNode GetCurrentNode();

        /// <summary>
        /// Gets active node scope configuration
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<int> GetActiveNodeScopeConfigurationAsync();
    }
}
