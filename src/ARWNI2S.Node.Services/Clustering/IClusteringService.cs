using ARWNI2S.Node.Data.Entities.Clustering;

namespace ARWNI2S.Node.Data.Services.Clustering
{
    /// <summary>
    /// BladeServer service interface
    /// </summary>
    public partial interface IClusteringService
    {
        /// <summary>
        /// Deletes a node
        /// </summary>
        /// <param name="server">Server</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteServerAsync(BladeServer server);

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the nodes
        /// </returns>
        Task<IList<BladeServer>> GetAllServersAsync();

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// The nodes
        /// </returns>
        IList<BladeServer> GetAllNodes();

        /// <summary>
        /// Gets a node 
        /// </summary>
        /// <param name="serverId">Server identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node
        /// </returns>
        Task<BladeServer> GetNodeByIdAsync(int serverId);

        /// <summary>
        /// Inserts a node
        /// </summary>
        /// <param name="server">Server</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertServerAsync(BladeServer server);

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="server">Server</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateServerAsync(BladeServer server);

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="server">Server</param>
        void UpdateNode(BladeServer server);

        /// <summary>
        /// Indicates whether a node contains a specified host
        /// </summary>
        /// <param name="server">Server</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        bool ContainsHostValue(BladeServer server, string host);

        /// <summary>
        /// Returns a list of names of not existing nodes
        /// </summary>
        /// <param name="serverIdsNames">The names and/or IDs of the node to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing nodes
        /// </returns>
        Task<string[]> GetNotExistingServersAsync(string[] serverIdsNames);
    }
}