using ARWNI2S.Infrastructure;
using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Entities.Clustering;
using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Services.Clustering;
using ARWNI2S.Node.Services.Common;

namespace ARWNI2S.Runtime
{
    /// <summary>
    /// NI2SNode context for application
    /// </summary>
    public partial class LocalNodeContext : INodeContext
    {
        private readonly NodeConfig _nodeConfig;
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IContextAccessor _contextAccessor;
        private readonly IRepository<NI2SNode> _nodeRepository;
        private readonly IClusteringService _clusterService;

        private NI2SNode _cachedNode;
        private int? _cachedActiveNodeScopeConfiguration;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="contextAccessor">Context accessor</param>
        /// <param name="nodeRepository">NI2SNode repository</param>
        /// <param name="clusterService">NI2SNode service</param>
        public LocalNodeContext(NI2SSettings ni2sSettings,
            IGenericAttributeService genericAttributeService,
            IContextAccessor contextAccessor,
            IRepository<NI2SNode> nodeRepository,
            IClusteringService clusterService)
        {
            _nodeConfig = ni2sSettings.Get<NodeConfig>();
            _genericAttributeService = genericAttributeService;
            _contextAccessor = contextAccessor;
            _nodeRepository = nodeRepository;
            _clusterService = clusterService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the local node
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<NI2SNode> GetLocalNodeAsync()
        {
            if (_cachedNode != null)
                return _cachedNode;

            _cachedNode = await _clusterService.GetNodeByNodeIdAsync(_nodeConfig.NodeId) ?? throw new NodeException("No node could be loaded");

            return _cachedNode;
        }

        /// <summary>
        /// Gets the current node
        /// </summary>
        public virtual NI2SNode GetCurrentNode()
        {
            if (_cachedNode != null)
                return _cachedNode;

            if (GetLocalNodeAsync() != null)
                return _cachedNode;

            //we cannot call async methods here. otherwise, an application can hang. so it's a workaround to avoid that
            var allNodes = _nodeRepository.GetAll(query =>
            {
                return from s in query orderby s.DisplayOrder, s.Id select s;
            }, _ => default, includeDeleted: false);

            _cachedNode = allNodes.FirstOrDefault(n => n.NodeId == _nodeConfig.NodeId) ?? throw new NodeException("No node could be loaded");

            return _cachedNode;
        }

        /// <summary>
        /// Gets active node scope configuration
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<int> GetActiveNodeScopeConfigurationAsync()
        {
            //TODO: 001 - REVIEW GET ONLY LOCAL NODE
            if (_cachedActiveNodeScopeConfiguration.HasValue)
                return _cachedActiveNodeScopeConfiguration.Value;

            //ensure that we have 2 (or more) nodes
            if ((await _clusterService.GetAllNodesAsync()).Count > 1)
            {
                //do not inject IWorkContext via constructor because it'll cause circular references
                var currentUser = await NodeEngineContext.Current.Resolve<IWorkContext>().GetCurrentUserAsync() as User;

                //try to get node identifier from attributes
                var nodeId = await _genericAttributeService
                    .GetAttributeAsync<int>(currentUser, UserDefaults.NodeScopeConfigurationAttribute);

                _cachedActiveNodeScopeConfiguration = (await _clusterService.GetNodeByIdAsync(nodeId))?.Id ?? 0;
            }
            else
                _cachedActiveNodeScopeConfiguration = 0;

            return _cachedActiveNodeScopeConfiguration ?? 0;
        }

        async Task<NI2SNode> INodeContext.GetCurrentNodeAsync() => await GetLocalNodeAsync();

        NI2SNode INodeContext.GetCurrentNode() => GetCurrentNode();

        #endregion
    }
}