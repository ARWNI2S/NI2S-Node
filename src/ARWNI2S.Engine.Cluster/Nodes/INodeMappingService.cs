using ARWNI2S.Cluster.Entities;
using ARWNI2S.Engine.Data;

namespace ARWNI2S.Cluster.Nodes
{
    /// <summary>
    /// NodeInfo mapping service interface
    /// </summary>
    public partial interface INodeMappingService
    {
        /// <summary>
        /// Apply node mapping to the passed query
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="query">Query to filter</param>
        /// <param name="nodeId">NodeInfo identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the filtered query
        /// </returns>
        Task<IQueryable<TEntity>> ApplyNodeMapping<TEntity>(IQueryable<TEntity> query, int nodeId) where TEntity : DataEntity, INodeMappingSupported;

        /// <summary>
        /// Deletes a node mapping record
        /// </summary>
        /// <param name="nodeMapping">NodeInfo mapping record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteNodeMappingAsync(NodeMapping nodeMapping);

        /// <summary>
        /// Gets node mapping records
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node mapping records
        /// </returns>
        Task<IList<NodeMapping>> GetNodeMappingsAsync<TEntity>(TEntity entity) where TEntity : DataEntity, INodeMappingSupported;

        /// <summary>
        /// Inserts a node mapping record
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="nodeId">NodeInfo id</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertNodeMappingAsync<TEntity>(TEntity entity, int nodeId) where TEntity : DataEntity, INodeMappingSupported;

        /// <summary>
        /// Find node identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node identifiers
        /// </returns>
        Task<int[]> GetNodesIdsWithAccessAsync<TEntity>(TEntity entity) where TEntity : DataEntity, INodeMappingSupported;

        /// <summary>
        /// Find node identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// The node identifiers
        /// </returns>
        int[] GetNodesIdsWithAccess<TEntity>(TEntity entity) where TEntity : DataEntity, INodeMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in the current node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains true - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync<TEntity>(TEntity entity) where TEntity : DataEntity, INodeMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in a node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="nodeId">NodeInfo identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains true - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync<TEntity>(TEntity entity, int nodeId) where TEntity : DataEntity, INodeMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in a node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="nodeId">NodeInfo identifier</param>
        /// <returns>
        /// True - authorized; otherwise, false
        /// </returns>
        bool Authorize<TEntity>(TEntity entity, int nodeId) where TEntity : DataEntity, INodeMappingSupported;
    }
}