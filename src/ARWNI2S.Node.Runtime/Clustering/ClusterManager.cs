using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Entities.Clustering;
using ARWNI2S.Node.Services.Clustering;

namespace ARWNI2S.Runtime.Clustering
{
    public sealed class ClusterManager
    {
        //private readonly NodeSettings _nodeSettings;
        private readonly LocalNodeContext _nodeContext;
        private readonly IClusteringService _clusteringService;
        private readonly INodeMappingService _nodeMappingService;

        public ClusterManager(NI2SSettings nodeSettings,
            LocalNodeContext nodeContext,
            IClusteringService clusteringService,
            INodeMappingService nodeMappingService)
        {
            //_nodeSettings = nodeSettings;
            _nodeContext = nodeContext;
            _clusteringService = clusteringService;
            _nodeMappingService = nodeMappingService;
        }

        // Método para que un nodo se una al clúster
        public async Task JoinClusterAsync(Guid? nodeId = null)
        {
            var node = nodeId.HasValue ? await _clusteringService.GetNodeByNodeIdAsync(nodeId.Value) ?? await _nodeContext.GetCurrentNodeAsync()
                : await _nodeContext.GetCurrentNodeAsync();

            if (node == null || nodeId.HasValue && node.NodeId != nodeId.Value)
            {
                node = new NI2SNode
                {
                    IpAddress = null,
                    CurrentState = NodeState.Offline,
                    DefaultLanguageId = 0,
                    DisplayOrder = (await _clusteringService.GetAllNodesAsync()).Max(n => n.DisplayOrder) + 1,
                    Hosts = Environment.MachineName,
                    NodeId = nodeId ?? new Guid(),
                    PublicPort = ClusteringDefaults.PublicPort,
                    RelayPort = ClusteringDefaults.RelayPort,
                    SslEnabled = false,

                };

                await _clusteringService.InsertNodeAsync(node);
            }

            node.Metadata = null;
            node.CurrentState = NodeState.Joining;
            await _clusteringService.UpdateNodeAsync(node);

            // TODO: JOIN PROCESS


            node.CurrentState = NodeState.Online;
            await _clusteringService.UpdateNodeAsync(node);
        }

        // TODO: Método para que un nodo salga del clúster
        public async Task LeaveClusterAsync(Guid? nodeId = null)
        {
            var node = nodeId.HasValue ? await _clusteringService.GetNodeByNodeIdAsync(nodeId.Value) ?? await _nodeContext.GetCurrentNodeAsync()
                : await _nodeContext.GetCurrentNodeAsync();


        }

        // TODO: Obtener el estado del clúster
        public ClusterStatus GetClusterStatus()
        {
            var node = _nodeContext.GetCurrentNode();

            var allNodes = _clusteringService.GetAllNodes();

            return new ClusterStatus
            {
                OnlineNodes = allNodes.Count(n => n.CurrentState == NodeState.Online),
                OfflineNodes = allNodes.Count(n => n.CurrentState == NodeState.Offline),
                NodesWithError = allNodes.Count(n => n.CurrentState == NodeState.Error),

                SpinningUpNodes = allNodes.Count(n => n.CurrentState == NodeState.Joining),
                SpinningDownNodes = allNodes.Count(n => n.CurrentState == NodeState.Leaving)
            };
        }

        // TODO: Actualiza el estado del clúster
        public async Task UpdateNodeStatusAsync(int runningEntities)
        {
            var node = await _nodeContext.GetCurrentNodeAsync();

        }

        // TODO: Obtener los detalles de un nodo
        public NI2SNode GetNodeDetails()
        {
            return _nodeContext.GetCurrentNode();
        }

        // TODO: Obtener los detalles de un nodo
        public async Task<NI2SNode> GetNodeDetailsAsync(int nodeId = 0)
        {
            if (nodeId == 0)
                return await _nodeContext.GetCurrentNodeAsync();
            return await _clusteringService.GetNodeByIdAsync(nodeId);
        }
    }
}
