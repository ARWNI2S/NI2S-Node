using ARWNI2S.Node.Data.Entities;
using ARWNI2S.Node.Data.Entities.Clustering;

namespace ARWNI2S.Node.Data.Services.Clustering
{
    /// <summary>
    /// Server mapping service interface
    /// </summary>
    public partial interface INodeMappingService
    {
        /// <summary>
        /// Apply server mapping to the passed query
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="query">Query to filter</param>
        /// <param name="serverId">Server identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the filtered query
        /// </returns>
        Task<IQueryable<TEntity>> ApplyNodeMapping<TEntity>(IQueryable<TEntity> query, int serverId) where TEntity : BaseDataEntity, INodeMappingSupported;

        /// <summary>
        /// Deletes a server mapping record
        /// </summary>
        /// <param name="serverMapping">Server mapping record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteNodeMappingAsync(NodeMapping serverMapping);

        /// <summary>
        /// Gets server mapping records
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the server mapping records
        /// </returns>
        Task<IList<NodeMapping>> GetNodeMappingsAsync<TEntity>(TEntity entity) where TEntity : BaseDataEntity, INodeMappingSupported;

        /// <summary>
        /// Inserts a server mapping record
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="serverId">Server id</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertNodeMappingAsync<TEntity>(TEntity entity, int serverId) where TEntity : BaseDataEntity, INodeMappingSupported;

        /// <summary>
        /// Find node identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node identifiers
        /// </returns>
        Task<int[]> GetServersIdsWithAccessAsync<TEntity>(TEntity entity) where TEntity : BaseDataEntity, INodeMappingSupported;

        /// <summary>
        /// Find node identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// The node identifiers
        /// </returns>
        int[] GetServersIdsWithAccess<TEntity>(TEntity entity) where TEntity : BaseDataEntity, INodeMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in the current node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync<TEntity>(TEntity entity) where TEntity : BaseDataEntity, INodeMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in a node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="serverId">Server identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync<TEntity>(TEntity entity, int serverId) where TEntity : BaseDataEntity, INodeMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in a node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="serverId">Server identifier</param>
        /// <returns>
        /// The rue - authorized; otherwise, false
        /// </returns>
        bool Authorize<TEntity>(TEntity entity, int serverId) where TEntity : BaseDataEntity, INodeMappingSupported;
    }
}