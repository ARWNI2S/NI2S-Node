using NetMQ;

namespace ARWNI2S.Cluster.Networking
{
    internal class ClusterNodeManager
    {
        private readonly NetMQBeacon _beacon;
        private readonly TimeSpan _deadNodeTimeout;
        private readonly Dictionary<string, DateTime> _activeNodes; // Key: Address, Value: LastSeen

        public event Action<string> NodeAdded;
        public event Action<string> NodeRemoved;

        public ClusterNodeManager(int broadcastPort, TimeSpan deadNodeTimeout)
        {
            _beacon = new NetMQBeacon();
            _deadNodeTimeout = deadNodeTimeout;
            _activeNodes = new Dictionary<string, DateTime>();

            _beacon.Configure(broadcastPort);
            _beacon.Subscribe("");
        }

        public void Start()
        {
            _beacon.ReceiveReady += OnBeaconReceived;

            NetMQTimer cleanupTimer = new(_deadNodeTimeout);
            cleanupTimer.Elapsed += ClearDeadNodes;

            NetMQPoller poller = new() { _beacon, cleanupTimer };
            poller.RunAsync();
        }

        public IEnumerable<string> GetActiveNodes()
        {
            return _activeNodes.Keys.ToList();
        }

        private void OnBeaconReceived(object sender, NetMQBeaconEventArgs e)
        {
            var message = _beacon.Receive();
            string nodeAddress = $"{message.PeerHost}:{message.String}";

            if (!_activeNodes.ContainsKey(nodeAddress))
            {
                _activeNodes[nodeAddress] = DateTime.Now;
                NodeAdded?.Invoke(nodeAddress);
            }
            else
            {
                _activeNodes[nodeAddress] = DateTime.Now;
            }
        }

        private void ClearDeadNodes(object sender, NetMQTimerEventArgs e)
        {
            var now = DateTime.Now;
            var deadNodes = _activeNodes.Where(n => now - n.Value > _deadNodeTimeout).Select(n => n.Key).ToList();

            foreach (var node in deadNodes)
            {
                _activeNodes.Remove(node);
                NodeRemoved?.Invoke(node);
            }
        }
    }
}
