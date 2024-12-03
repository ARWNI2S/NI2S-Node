using ARWNI2S.Data.Entities.Clustering;

namespace ARWNI2S.Services.Clustering
{
    /// <summary>
    /// NI2SNode service interface
    /// </summary>
    public partial interface IClusteringService
    {
        /// <summary>
        /// Deletes a node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteNodeAsync(ClusterNode node);

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the nodes
        /// </returns>
        Task<IList<ClusterNode>> GetAllNodesAsync(NodeState? nodeState = null);

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// The nodes
        /// </returns>
        IList<ClusterNode> GetAllNodes();

        /// <summary>
        /// Gets a node 
        /// </summary>
        /// <param name="nodeId">Node identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node
        /// </returns>
        Task<ClusterNode> GetNodeByIdAsync(int nodeId);

        /// <summary>
        /// Gets a node 
        /// </summary>
        /// <param name="nodeId">Node identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node
        /// </returns>
        Task<ClusterNode> GetNodeByNodeIdAsync(Guid nodeId);

        /// <summary>
        /// Inserts a node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertNodeAsync(ClusterNode node);

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateNodeAsync(ClusterNode node);

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="node">Node</param>
        void UpdateNode(ClusterNode node);

        /// <summary>
        /// Indicates whether a node contains a specified host
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        bool ContainsHostValue(ClusterNode node, string host);

        /// <summary>
        /// Gets current cluster map registration
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the nodes
        /// </returns>
        Task<ClusterMap> GetClusterMapRegistrationAsync();

        /// <summary>
        /// Gets the actual cluster status
        /// </summary>
        /// <returns>A <see cref="ClusterStatus"/> containing updated data</returns>
        ClusterStatus GetClusterStatus();

        /// <summary>
        /// Returns a list of names of not existing nodes
        /// </summary>
        /// <param name="nodeIdsNames">The names and/or IDs of the node to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing nodes
        /// </returns>
        Task<Guid[]> GetNotExistingNodesAsync(string[] nodeIdsNames);
    }
}