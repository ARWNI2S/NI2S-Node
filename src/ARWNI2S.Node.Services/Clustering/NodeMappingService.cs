using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Data.Entities;
using ARWNI2S.Node.Data.Entities.Clustering;
using ARWNI2S.Node.Data.Extensions;

namespace ARWNI2S.Node.Data.Services.Clustering
{
    /// <summary>
    /// BladeServer mapping service
    /// </summary>
    public partial class NodeMappingService : INodeMappingService
    {
        #region Fields

        private readonly NodeSettings _nodeSettings;
        private readonly IRepository<NodeMapping> _serverMappingRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IServerContext _serverContext;

        #endregion

        #region Ctor

        public NodeMappingService(NodeSettings nodeSettings,
            //NodeSettings nodeSettings,
            IRepository<NodeMapping> serverMappingRepository,
            IStaticCacheManager staticCacheManager,
            IServerContext serverContext)
        {
            _nodeSettings = nodeSettings;
            _serverMappingRepository = serverMappingRepository;
            _staticCacheManager = staticCacheManager;
            _serverContext = serverContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Inserts a server mapping record
        /// </summary>
        /// <param name="serverMapping">Server mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InsertNodeMappingAsync(NodeMapping serverMapping)
        {
            await _serverMappingRepository.InsertAsync(serverMapping);
        }

        /// <summary>
        /// Get a value indicating whether a server mapping exists for an entity type
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue if exists; otherwise false
        /// </returns>
        protected virtual async Task<bool> IsEntityMappingExistsAsync<TEntity>() where TEntity : BaseDataEntity, INodeMappingSupported
        {
            var entityName = typeof(TEntity).Name;
            var key = _staticCacheManager.PrepareKeyForDefaultCache(ClusteringServiceDefaults.NodeMappingExistsCacheKey, entityName);

            var query = from sm in _serverMappingRepository.Table
                        where sm.EntityName == entityName
                        select sm.ServerId;

            return await _staticCacheManager.GetAsync(key, query.Any);
        }

        #endregion

        #region Methods

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
        public virtual async Task<IQueryable<TEntity>> ApplyNodeMapping<TEntity>(IQueryable<TEntity> query, int serverId)
            where TEntity : BaseDataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(query);

            if (serverId == 0 || _nodeSettings.IgnoreServerLimitations || !await IsEntityMappingExistsAsync<TEntity>())
                return query;

            return from entity in query
                   where !entity.LimitedToServers || _serverMappingRepository.Table.Any(sm =>
                         sm.EntityName == typeof(TEntity).Name && sm.EntityId == entity.Id && sm.ServerId == serverId)
                   select entity;
        }

        /// <summary>
        /// Deletes a server mapping record
        /// </summary>
        /// <param name="serverMapping">Server mapping record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteNodeMappingAsync(NodeMapping serverMapping)
        {
            await _serverMappingRepository.DeleteAsync(serverMapping);
        }

        /// <summary>
        /// Gets server mapping records
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the server mapping records
        /// </returns>
        public virtual async Task<IList<NodeMapping>> GetNodeMappingsAsync<TEntity>(TEntity entity) where TEntity : BaseDataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(entity);

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(ClusteringServiceDefaults.NodeMappingsCacheKey, entityId, entityName);

            var query = from sm in _serverMappingRepository.Table
                        where sm.EntityId == entityId &&
                        sm.EntityName == entityName
                        select sm;

            var serverMappings = await _staticCacheManager.GetAsync(key, async () => await query.ToListAsync());

            return serverMappings;
        }

        /// <summary>
        /// Inserts a server mapping record
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="serverId">Server id</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertNodeMappingAsync<TEntity>(TEntity entity, int serverId) where TEntity : BaseDataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(entity);

            ArgumentOutOfRangeException.ThrowIfZero(serverId);

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var serverMapping = new NodeMapping
            {
                EntityId = entityId,
                EntityName = entityName,
                ServerId = serverId
            };

            await InsertNodeMappingAsync(serverMapping);
        }

        /// <summary>
        /// Find node identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node identifiers
        /// </returns>
        public virtual async Task<int[]> GetServersIdsWithAccessAsync<TEntity>(TEntity entity) where TEntity : BaseDataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(entity);

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(ClusteringServiceDefaults.NodeMappingIdsCacheKey, entityId, entityName);

            var query = from sm in _serverMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm.ServerId;

            return await _staticCacheManager.GetAsync(key, () => query.ToArray());
        }

        /// <summary>
        /// Find node identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// The node identifiers
        /// </returns>
        public virtual int[] GetServersIdsWithAccess<TEntity>(TEntity entity) where TEntity : BaseDataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(entity);

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(ClusteringServiceDefaults.NodeMappingIdsCacheKey, entityId, entityName);

            var query = from sm in _serverMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm.ServerId;

            return _staticCacheManager.Get(key, () => query.ToArray());
        }

        /// <summary>
        /// Authorize whether entity could be accessed in the current node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        public virtual async Task<bool> AuthorizeAsync<TEntity>(TEntity entity) where TEntity : BaseDataEntity, INodeMappingSupported
        {
            var node = (BladeServer)await _serverContext.GetCurrentServerAsync();

            return await AuthorizeAsync(entity, node.Id);
        }

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
        public virtual async Task<bool> AuthorizeAsync<TEntity>(TEntity entity, int serverId) where TEntity : BaseDataEntity, INodeMappingSupported
        {
            if (entity == null)
                return false;

            if (serverId == 0)
                //return true if no node specified/found
                return true;

            if (_nodeSettings.IgnoreServerLimitations)
                return true;

            if (!entity.LimitedToServers)
                return true;

            foreach (var serverIdWithAccess in await GetServersIdsWithAccessAsync(entity))
                if (serverId == serverIdWithAccess)
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        /// <summary>
        /// Authorize whether entity could be accessed in a node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports server mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="serverId">Server identifier</param>
        /// <returns>
        /// The rue - authorized; otherwise, false
        /// </returns>
        public virtual bool Authorize<TEntity>(TEntity entity, int serverId) where TEntity : BaseDataEntity, INodeMappingSupported
        {
            if (entity == null)
                return false;

            if (serverId == 0)
                //return true if no node specified/found
                return true;

            if (_nodeSettings.IgnoreServerLimitations)
                return true;

            if (!entity.LimitedToServers)
                return true;

            foreach (var serverIdWithAccess in GetServersIdsWithAccess(entity))
                if (serverId == serverIdWithAccess)
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        #endregion
    }
}