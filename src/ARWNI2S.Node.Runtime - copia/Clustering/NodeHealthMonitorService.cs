using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Entities.Clustering;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Services.Clustering;
using ARWNI2S.Runtime.Clustering.Extensions;
using ARWNI2S.Runtime.Clustering.Messages;
using System.Net.Sockets;

namespace ARWNI2S.Runtime.Clustering
{
    public class NodeHealthMonitorService : NodeRuntimeService
    {
        protected readonly ClusteringSettings _settings;
        protected readonly IClusteringService _clusteringService;
        //protected readonly NodeConnectionManager _connectionManager;
        protected readonly NodeClient _nodeClient;

        protected IList<string> knownNodes = [];

        protected ClusterMap clusterMap;

        public NodeHealthMonitorService(ClusteringSettings settings,
            IClusteringService clusteringService,
            //NodeConnectionManager connectionManager,
            INodeClientFactory clientFactory)
        {
            _settings = settings;
            _clusteringService = clusteringService;
            //_connectionManager = connectionManager;
            _nodeClient = clientFactory.GetOrCreateClient<NodeHealthMonitorService>();
        }

        #region Utilities


        #endregion

        #region Runtime Service

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            knownNodes = EngineContext.Current.Resolve<NI2SSettings>()?.Get<ClusterConfig>()?.KnownNodes.Split(',');

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RefreshClusterMapAsync(stoppingToken);
                await PingNodesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(_settings.HealthMonitorFrequencySeconds), stoppingToken);
            }
        }

        #endregion

        private async Task RefreshClusterMapAsync(CancellationToken stoppingToken)
        {
            clusterMap = await _clusteringService.GetClusterMapRegistrationAsync();
        }

        private async Task PingNodesAsync(CancellationToken stoppingToken)
        {
            var beaconNodes = clusterMap.OnlineNodes.Where(node => knownNodes.Contains(node.NodeId.ToString())).ToList();

            if (beaconNodes.Count == 0)
                beaconNodes = clusterMap.OnlineNodes;

            var unreachableNodes = knownNodes
                .Select(Guid.Parse)
                .Except(beaconNodes.Select(node => node.NodeId))
                .ToList();

            foreach (var node in beaconNodes)
            {
                if (!await PingNodeAsync(node, stoppingToken))
                {
                    unreachableNodes.Add(node.NodeId);
                }
            }

            if (unreachableNodes.Count != 0)
            {
                await ConductVotingAsync(beaconNodes, unreachableNodes, stoppingToken);
            }
        }

        private async Task<bool> PingNodeAsync(NI2SNode node, CancellationToken stoppingToken)
        {
            try
            {
                return await Task.FromResult(true); //_nodeClient.ConnectAsync(new IPEndPoint(IPAddress.Parse(node.IpAddress), node.RelayPort), stoppingToken);
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AccessDenied || e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    //TODO: error control
                }

                return false;
            }
        }

        private async Task ConductVotingAsync(List<NI2SNode> beaconNodes, List<Guid> unreachableNodes, CancellationToken stoppingToken)
        {
            foreach (var unreachableNodeId in unreachableNodes)
            {
                var votes = new List<bool>();
                await Parallel.ForEachAsync(beaconNodes, async (beacon, stoppingToken) =>
                {
                    if (beacon.NodeId == unreachableNodeId) return;
                    // Intentar hacer ping al nodo problemático desde otros nodos
                    if (await RequestQuorumAsync(beacon, unreachableNodeId, stoppingToken))
                    {
                        votes.Add(true);
                    }
                    else
                    {
                        votes.Add(false);
                    }
                });

                // Si la mayoría de nodos votan como no conectado, actualizar el estado en la base de datos
                bool isAlive = votes.Count(v => v) > votes.Count / 2;
                await UpdateNodeStatusAsync(unreachableNodeId, isAlive);
            }
        }

        private async Task<bool> RequestQuorumAsync(NI2SNode votingNode, Guid targetNode, CancellationToken stoppingToken)
        {
            // Simulamos una llamada remota al nodo votingNode para que haga ping a targetNode
            // En una implementación real, sería una comunicación a través de sockets o un RPC
            try
            {
                bool connected = await Task.FromResult(true); //_nodeClient.ConnectAsync(new IPEndPoint(IPAddress.Parse(votingNode.IpAddress), votingNode.RelayPort), stoppingToken);

                await _nodeClient.SendQuorumRequestAsync(new QuorumRequest(targetNode, votingNode));

                var data = await _nodeClient.ReceiveQuorumResponseAsync();

                return data.HasErrors && data.Vote;
            }
            catch
            {
                return false;
            }
        }

        private async Task UpdateNodeStatusAsync(Guid nodeId, bool isAlive)
        {
            var node = await _clusteringService.GetNodeByNodeIdAsync(nodeId);
            if (node != null)
            {
                node.CurrentState = isAlive ? NodeState.Online : NodeState.Offline;

                await _clusteringService.UpdateNodeAsync(node);
            }
        }

        public override void Dispose()
        {
            //_nodeClient.Dispose();
            base.Dispose();
        }
    }
}
