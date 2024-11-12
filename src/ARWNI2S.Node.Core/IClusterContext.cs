using ARWNI2S.Node.Core.Entities.Clustering;

namespace ARWNI2S.Node.Core
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
        Task<ClusterNode> GetCurrentNodeAsync();

        /// <summary>
        /// Gets the current node
        /// </summary>
        ClusterNode GetCurrentNode();

        /// <summary>
        /// Gets active node scope configuration
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<int> GetActiveNodeScopeConfigurationAsync();
    }
}
