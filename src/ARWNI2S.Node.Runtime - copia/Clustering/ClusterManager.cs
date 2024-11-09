using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Entities.Clustering;
using ARWNI2S.Node.Core.Network;
using ARWNI2S.Node.Services.Clustering;
using System.Net;

namespace ARWNI2S.Runtime.Clustering
{
    public sealed class ClusterManager
    {
        private readonly NodeConfig _nodeConfig;
        private readonly INodeContext _nodeContext;
        private readonly IClusteringService _clusteringService;
        private readonly INodeMappingService _nodeMappingService;
        private readonly INI2SNetHelper _nodeHelper;

        public ClusterManager(NI2SSettings nodeSettings,
            INodeContext nodeContext,
            IClusteringService clusteringService,
            INodeMappingService nodeMappingService,
            INodeEventPublisher eventPublisher,
            INI2SNetHelper nodeHelper)
        {
            //_nodeSettings = nodeSettings;
            _nodeConfig = nodeSettings.Get<NodeConfig>();
            _nodeContext = nodeContext;
            _clusteringService = clusteringService;
            _nodeMappingService = nodeMappingService;
            _eventPublisher = eventPublisher;
            _nodeHelper = nodeHelper;
        }

        /// <summary>
        /// Método para unir el nodo local al clúster
        /// </summary>
        public async Task JoinClusterAsync()
        {
            var node = await _nodeContext.GetCurrentNodeAsync();

            if (node == null)
            {
                node = new NI2SNode
                {
                    Name = _nodeConfig.NodeName,
                    CurrentState = NodeState.Offline,
                    Hosts = $"{Dns.GetHostName()}:{_nodeConfig.Port}",
                    IpAddress = _nodeHelper.GetCurrentIpAddress(),
                    NodeId = _nodeConfig.NodeId,
                    PublicPort = _nodeConfig.Port,
                    RelayPort = _nodeConfig.RelayPort,
                    SslEnabled = false,
                    DefaultLanguageId = 0,
                    DisplayOrder = (await _clusteringService.GetAllNodesAsync()).Max(n => n.DisplayOrder) + 1,
                };

                await _clusteringService.InsertNodeAsync(node);
            }

            node.Metadata = null;
            node.CurrentState = NodeState.Joining;
            await _clusteringService.UpdateNodeAsync(node);

        }

        /// <summary>
        /// Metodo para que el nodo local deje el clúster.
        /// </summary>
        public async Task LeaveClusterAsync()
        {
            var node = await _nodeContext.GetCurrentNodeAsync();
            if (node != null)
            {
                node.CurrentState = NodeState.Leaving;
                await _clusteringService.UpdateNodeAsync(node);
            }
        }

        /// <summary>
        /// Obtener el estado mas reciente del cluster
        /// </summary>
        /// <returns>El estado real del cluster</returns>
        public ClusterStatus GetClusterStatus() => _clusteringService.GetClusterStatus();

        /// <summary>
        /// Actualiza el estado del clúster local
        /// </summary>
        /// <returns></returns>
        public async Task UpdateNodeStatusAsync(int? runningEntities = null)
        {
            var node = await _nodeContext.GetCurrentNodeAsync();
            if (node != null)
            {
                if (node.CurrentState == NodeState.Joining)
                    node.CurrentState = NodeState.Online;

                if (node.CurrentState == NodeState.Online)
                {
                    // DO UPDATE HERE IF NEEDED
                }

                if (node.CurrentState == NodeState.Leaving || node.CurrentState == NodeState.Error)
                    node.CurrentState = NodeState.Offline;

                if (node.CurrentState == NodeState.Offline)
                {
                    node.IpAddress = null;
                    node.AverageEntities = 0;
                    node.Metadata = null;
                }

                await _clusteringService.UpdateNodeAsync(node);
            }
        }

        public NI2SNode GetLocalNode() => _nodeContext.GetCurrentNode();

        public async Task<NI2SNode> GetNodeDetailsAsync(int nodeId = 0)
        {
            if (nodeId == 0)
                return await _nodeContext.GetCurrentNodeAsync();
            return await _clusteringService.GetNodeByIdAsync(nodeId);
        }

        public async Task<NI2SNode> GetNodeDetailsAsync(Guid nodeId) => await _clusteringService.GetNodeByNodeIdAsync(nodeId);
    }
}
