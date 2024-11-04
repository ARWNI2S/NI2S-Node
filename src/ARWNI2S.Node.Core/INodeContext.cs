using ARWNI2S.Node.Core.Entities.Clustering;

namespace ARWNI2S.Node.Core
{
    /// <summary>
    /// Node context
    /// </summary>
    public interface INodeContext
    {
        /// <summary>
        /// Gets the current node
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<NI2SNode> GetCurrentNodeAsync();

        /// <summary>
        /// Gets the current node
        /// </summary>
        NI2SNode GetCurrentNode();

        /// <summary>
        /// Gets active node scope configuration
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<int> GetActiveNodeScopeConfigurationAsync();
    }
}
