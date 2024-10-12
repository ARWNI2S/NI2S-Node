using ARWNI2S.Node.Data.Entities.Clustering;

namespace ARWNI2S.Node.Services.Clustering
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
        Task DeleteNodeAsync(NI2SNode node);

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the nodes
        /// </returns>
        Task<IList<NI2SNode>> GetAllNodesAsync();

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// The nodes
        /// </returns>
        IList<NI2SNode> GetAllNodes();

        /// <summary>
        /// Gets a node 
        /// </summary>
        /// <param name="nodeId">Node identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node
        /// </returns>
        Task<NI2SNode> GetNodeByIdAsync(int nodeId);

        /// <summary>
        /// Inserts a node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertNodeAsync(NI2SNode node);

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateNodeAsync(NI2SNode node);

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="node">Node</param>
        void UpdateNode(NI2SNode node);

        /// <summary>
        /// Indicates whether a node contains a specified host
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        bool ContainsHostValue(NI2SNode node, string host);

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