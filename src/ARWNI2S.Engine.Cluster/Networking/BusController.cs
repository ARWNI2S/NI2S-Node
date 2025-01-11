using NetMQ;

namespace ARWNI2S.Cluster.Networking
{
    internal class BusController
    {
        private readonly ClusterNodeManager _nodeManager;
        private readonly MessageTransporter _transporter;
        private readonly CommandProcessor _commandProcessor;

        public BusController(int broadcastPort, TimeSpan deadNodeTimeout)
        {
            _nodeManager = new ClusterNodeManager(broadcastPort, deadNodeTimeout);
            _transporter = new MessageTransporter();
            _commandProcessor = new CommandProcessor(_transporter, _nodeManager);

            _nodeManager.NodeAdded += OnNodeAdded;
            _nodeManager.NodeRemoved += OnNodeRemoved;
            _transporter.MessageReceived += OnMessageReceived;
        }

        public void Start()
        {
            _nodeManager.Start();
            _transporter.Start();
        }

        public void ExecuteCommand(string command, params object[] args)
        {
            _commandProcessor.ProcessCommand(command, args);
        }

        private void OnNodeAdded(string nodeAddress)
        {
            Console.WriteLine($"Node added: {nodeAddress}");
            var message = new NetMQMessage();
            message.Append("NodeJoined");
            message.Append(nodeAddress);
            _transporter.SendMessage(message);
        }

        private void OnNodeRemoved(string nodeAddress)
        {
            Console.WriteLine($"Node removed: {nodeAddress}");
        }

        private void OnMessageReceived(NetMQMessage message)
        {
            _commandProcessor.ProcessMessage(message);
        }
    }
}
