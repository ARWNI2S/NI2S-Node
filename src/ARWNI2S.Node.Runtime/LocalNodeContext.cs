using ARWNI2S.Infrastructure.Engine;
using ARWNI2S.Node.Core;
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
    public partial class LocalNodeContext : IClusteringContext
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IEngineContextAccessor _engineContextAccessor;
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
        /// <param name="engineContextAccessor">HTTP context accessor</param>
        /// <param name="nodeRepository">NI2SNode repository</param>
        /// <param name="clusterService">NI2SNode service</param>
        public LocalNodeContext(
            IGenericAttributeService genericAttributeService,
            IEngineContextAccessor engineContextAccessor,
            IRepository<NI2SNode> nodeRepository,
            IClusteringService clusterService)
        {
            _genericAttributeService = genericAttributeService;
            _engineContextAccessor = engineContextAccessor;
            _nodeRepository = nodeRepository;
            _clusterService = clusterService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current node
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<NI2SNode> GetCurrentNodeAsync()
        {
            //TODO: 001 - REVIEW GET ONLY LOCAL NODE
            if (_cachedNode != null)
                return _cachedNode;

            //try to determine the current node by HOST header
            string host = _engineContextAccessor.EngineContext?.LocalHost;

            var allNodes = await _clusterService.GetAllNodesAsync();
            var node = allNodes.FirstOrDefault(s => _clusterService.ContainsHostValue(s, host));

            //load the first found node
            node ??= allNodes.FirstOrDefault();

            _cachedNode = node ?? throw new NodeException("No node could be loaded");

            return _cachedNode;
        }

        /// <summary>
        /// Gets the current node
        /// </summary>
        public virtual NI2SNode GetCurrentNode()
        {
            //TODO: 001 - REVIEW GET ONLY LOCAL NODE
            if (_cachedNode != null)
                return _cachedNode;

            //try to determine the current node by HOST header
            string host = _engineContextAccessor.EngineContext?.LocalHost;

            //we cannot call async methods here. otherwise, an application can hang. so it's a workaround to avoid that
            var allNodes = _nodeRepository.GetAll(query =>
            {
                return from s in query orderby s.DisplayOrder, s.Id select s;
            }, _ => default, includeDeleted: false);

            var node = allNodes.FirstOrDefault(s => _clusterService.ContainsHostValue(s, host));

            //load the first found node
            node ??= allNodes.FirstOrDefault();

            _cachedNode = node ?? throw new NodeException("No node could be loaded");

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
                var currentUser = (User)await NodeEngineContext.Current.Resolve<IWorkContext>().GetCurrentUserAsync();

                //try to get node identifier from attributes
                var nodeId = await _genericAttributeService
                    .GetAttributeAsync<int>(currentUser, UserDefaults.AdminAreaNodeScopeConfigurationAttribute);

                _cachedActiveNodeScopeConfiguration = (await _clusterService.GetNodeByIdAsync(nodeId))?.Id ?? 0;
            }
            else
                _cachedActiveNodeScopeConfiguration = 0;

            return _cachedActiveNodeScopeConfiguration ?? 0;
        }

        async Task<NI2SNode> IClusteringContext.GetCurrentNodeAsync() => await GetCurrentNodeAsync();

        NI2SNode IClusteringContext.GetCurrentNode() => GetCurrentNode();

        #endregion
    }
}