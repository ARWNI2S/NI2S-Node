using ARWNI2S.Cluster.Entities;

namespace ARWNI2S.Cluster.Nodes
{
    /// <summary>
    /// NodeInfo service interface
    /// </summary>
    public partial interface INodeService
    {
        /// <summary>
        /// Deletes a node
        /// </summary>
        /// <param name="node">NodeInfo</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteNodeAsync(Node node);

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the nodes
        /// </returns>
        Task<IList<Node>> GetAllNodesAsync();

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// The nodes
        /// </returns>
        IList<Node> GetAllNodes();

        /// <summary>
        /// Gets a node 
        /// </summary>
        /// <param name="id">NodeInfo identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node
        /// </returns>
        Task<Node> GetNodeByIdAsync(int id);

        /// <summary>
        /// Gets a node 
        /// </summary>
        /// <param name="nodeId">NodeInfo identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node
        /// </returns>
        Task<Node> GetNodeByNodeIdAsync(Guid nodeId);

        /// <summary>
        /// Inserts a node
        /// </summary>
        /// <param name="node">NodeInfo</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertNodeAsync(Node node);

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="node">NodeInfo</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateNodeAsync(Node node);

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="node">NodeInfo</param>
        void UpdateNode(Node node);

        ///// <summary>
        ///// Indicates whether a node contains a specified host
        ///// </summary>
        ///// <param name="node">NodeInfo</param>
        ///// <param name="host">Host</param>
        ///// <returns>true - contains, false - no</returns>
        //bool ContainsHostValue(Node node, string host);

        /// <summary>
        /// Returns a list of names of not existing nodes
        /// </summary>
        /// <param name="nodeIdsNames">The names and/or IDs of the node to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing nodes
        /// </returns>
        Task<string[]> GetNotExistingNodesAsync(string[] nodeIdsNames);
    }
}