using ARWNI2S.Caching;
using ARWNI2S.Cluster.Entities;
using ARWNI2S.Engine.Caching;
using ARWNI2S.Engine.Data;
using LinqToDB;

namespace ARWNI2S.Cluster.Nodes
{
    /// <summary>
    /// NodeInfo mapping service
    /// </summary>
    public partial class NodeMappingService : INodeMappingService
    {
        #region Fields

        protected readonly ClusterSettings _clusterSettings;
        protected readonly IRepository<NodeMapping> _nodeMappingRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly INodeContext _nodeContext;

        #endregion

        #region Ctor

        public NodeMappingService(ClusterSettings clusterSettings,
            IRepository<NodeMapping> nodeMappingRepository,
            IStaticCacheManager staticCacheManager,
            INodeContext nodeContext)
        {
            _clusterSettings = clusterSettings;
            _nodeMappingRepository = nodeMappingRepository;
            _staticCacheManager = staticCacheManager;
            _nodeContext = nodeContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Inserts a node mapping record
        /// </summary>
        /// <param name="nodeMapping">NodeInfo mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InsertNodeMappingAsync(NodeMapping nodeMapping)
        {
            await _nodeMappingRepository.InsertAsync(nodeMapping);
        }

        /// <summary>
        /// Get a value indicating whether a node mapping exists for an entity type
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains true if exists; otherwise false
        /// </returns>
        protected virtual async Task<bool> IsEntityMappingExistsAsync<TEntity>() where TEntity : DataEntity, INodeMappingSupported
        {
            var entityName = typeof(TEntity).Name;
            var key = _staticCacheManager.PrepareKeyForDefaultCache(NodeDefaults.NodeMappingExistsCacheKey, entityName);

            var query = from sm in _nodeMappingRepository.Table
                        where sm.EntityName == entityName
                        select sm.NodeId;

            return await _staticCacheManager.GetAsync(key, query.Any);
        }

        #endregion

        #region Methods

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
        public virtual async Task<IQueryable<TEntity>> ApplyNodeMapping<TEntity>(IQueryable<TEntity> query, int nodeId)
            where TEntity : DataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(query);

            if (nodeId == 0 || _clusterSettings.IgnoreNodeLimitations || !await IsEntityMappingExistsAsync<TEntity>())
                return query;

            return from entity in query
                   where !entity.MappedToNodes || _nodeMappingRepository.Table.Any(sm =>
                       sm.EntityName == typeof(TEntity).Name && sm.EntityId == entity.Id && sm.NodeId == nodeId)
                   select entity;
        }

        /// <summary>
        /// Deletes a node mapping record
        /// </summary>
        /// <param name="nodeMapping">NodeInfo mapping record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteNodeMappingAsync(NodeMapping nodeMapping)
        {
            await _nodeMappingRepository.DeleteAsync(nodeMapping);
        }

        /// <summary>
        /// Gets node mapping records
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node mapping records
        /// </returns>
        public virtual async Task<IList<NodeMapping>> GetNodeMappingsAsync<TEntity>(TEntity entity) where TEntity : DataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(entity);

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(NodeDefaults.NodeMappingsCacheKey, entityId, entityName);

            var query = from sm in _nodeMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm;

            var nodeMappings = await _staticCacheManager.GetAsync(key, async () => await query.ToListAsync());

            return nodeMappings;
        }

        /// <summary>
        /// Inserts a node mapping record
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="nodeId">NodeInfo id</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertNodeMappingAsync<TEntity>(TEntity entity, int nodeId) where TEntity : DataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (nodeId == 0)
                throw new ArgumentOutOfRangeException(nameof(nodeId));

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var nodeMapping = new NodeMapping
            {
                EntityId = entityId,
                EntityName = entityName,
                NodeId = nodeId
            };

            await InsertNodeMappingAsync(nodeMapping);
        }

        /// <summary>
        /// Find node identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node identifiers
        /// </returns>
        public virtual async Task<int[]> GetNodesIdsWithAccessAsync<TEntity>(TEntity entity) where TEntity : DataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(entity);

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(NodeDefaults.NodeMappingIdsCacheKey, entityId, entityName);

            var query = from sm in _nodeMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm.NodeId;

            return await _staticCacheManager.GetAsync(key, () => query.ToArrayAsync());
        }

        /// <summary>
        /// Find node identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// The node identifiers
        /// </returns>
        public virtual int[] GetNodesIdsWithAccess<TEntity>(TEntity entity) where TEntity : DataEntity, INodeMappingSupported
        {
            ArgumentNullException.ThrowIfNull(entity);

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(NodeDefaults.NodeMappingIdsCacheKey, entityId, entityName);

            var query = from sm in _nodeMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm.NodeId;

            return _staticCacheManager.Get(key, () => query.ToArray());
        }

        /// <summary>
        /// Authorize whether entity could be accessed in the current node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains true - authorized; otherwise, false
        /// </returns>
        public virtual async Task<bool> AuthorizeAsync<TEntity>(TEntity entity) where TEntity : DataEntity, INodeMappingSupported
        {
            var node = (Node)await _nodeContext.GetCurrentNodeAsync();

            return await AuthorizeAsync(entity, node.Id);
        }

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
        public virtual async Task<bool> AuthorizeAsync<TEntity>(TEntity entity, int nodeId) where TEntity : DataEntity, INodeMappingSupported
        {
            if (entity == null)
                return false;

            if (nodeId == 0)
                //return true if no node specified/found
                return true;

            if (_clusterSettings.IgnoreNodeLimitations)
                return true;

            if (!entity.MappedToNodes)
                return true;

            foreach (var nodeIdWithAccess in await GetNodesIdsWithAccessAsync(entity))
                if (nodeId == nodeIdWithAccess)
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        /// <summary>
        /// Authorize whether entity could be accessed in a node (mapped to this node)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports node mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="nodeId">NodeInfo identifier</param>
        /// <returns>
        /// True - authorized; otherwise, false
        /// </returns>
        public virtual bool Authorize<TEntity>(TEntity entity, int nodeId) where TEntity : DataEntity, INodeMappingSupported
        {
            if (entity == null)
                return false;

            if (nodeId == 0)
                //return true if no node specified/found
                return true;

            if (_clusterSettings.IgnoreNodeLimitations)
                return true;

            if (!entity.MappedToNodes)
                return true;

            foreach (var nodeIdWithAccess in GetNodesIdsWithAccess(entity))
                if (nodeId == nodeIdWithAccess)
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        #endregion
    }
}