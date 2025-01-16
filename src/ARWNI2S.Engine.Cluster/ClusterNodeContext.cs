using ARWNI2S.Cluster.Configuration;
using ARWNI2S.Cluster.Entities;
using ARWNI2S.Cluster.Nodes;
using ARWNI2S.Configuration;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Data;
using ARWNI2S.Environment;

namespace ARWNI2S.Cluster
{
    /// <summary>
    /// Node context for web application
    /// </summary>
    public partial class ClusterNodeContext : INodeContext
    {
        #region Fields

        protected readonly NI2SSettings _settings;
        protected readonly INiisContextAccessor _contextAccessor;
        protected readonly IRepository<Node> _nodeRepository;
        protected readonly INodeService _nodeService;

        protected Node _cachedNode;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="contextAccessor">HTTP context accessor</param>
        /// <param name="nodeRepository">Node repository</param>
        /// <param name="nodeService">Node service</param>
        public ClusterNodeContext(NI2SSettings settings,
            INiisContextAccessor contextAccessor,
            IRepository<Node> nodeRepository,
            INodeService nodeService)
        {
            _settings = settings;
            _contextAccessor = contextAccessor;
            _nodeRepository = nodeRepository;
            _nodeService = nodeService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current node
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<Node> GetCurrentNodeAsync()
        {
            if (_cachedNode != null)
                return _cachedNode;

            //try to determine the current node by local configuration
            var nodeId = _settings.Get<NodeConfig>().NodeId; //_contextAccessor.HttpContext?.Request.Headers[HeaderNames.Host];

            var node = await _nodeService.GetNodeByNodeIdAsync(nodeId);

            _cachedNode = node ?? throw new Exception("No node could be loaded");

            return _cachedNode;
        }

        async Task<INiisNode> INodeContext.GetCurrentNodeAsync() =>  await GetCurrentNodeAsync();

        /// <summary>
        /// Gets the current node
        /// </summary>
        public virtual Node GetCurrentNode()
        {
            if (_cachedNode != null)
                return _cachedNode;

            //try to determine the current node by HOST header
            var nodeId = _settings.Get<NodeConfig>().NodeId;

            //we cannot call async methods here. otherwise, an application can hang. so it's a workaround to avoid that
            var allNodes = _nodeRepository.GetAll(query =>
            {
                return from s in query orderby s.Id select s;
            }, _ => default, includeDeleted: false);

            var node = allNodes.FirstOrDefault(n => n.NodeId == nodeId);

            _cachedNode = node ?? throw new Exception("No node could be loaded");

            return _cachedNode;
        }

        INiisNode INodeContext.GetCurrentNode() => GetCurrentNode();

        #endregion
    }
}