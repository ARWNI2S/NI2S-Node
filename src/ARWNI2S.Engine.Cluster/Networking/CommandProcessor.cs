using NetMQ;

namespace ARWNI2S.Cluster.Networking
{
    internal class CommandProcessor
    {
        private readonly MessageTransporter _transporter;
        private readonly ClusterNodeManager _nodeManager;

        public CommandProcessor(MessageTransporter transporter, ClusterNodeManager nodeManager)
        {
            _transporter = transporter;
            _nodeManager = nodeManager;
        }

        /// <summary>
        /// Procesa comandos desde el hilo principal.
        /// </summary>
        public void ProcessCommand(string command, params object[] args)
        {
            switch (command)
            {
                case "PublishMessage":
                    PublishMessage(args);
                    break;

                case "GetActiveNodes":
                    GetActiveNodes();
                    break;

                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }

        /// <summary>
        /// Procesa mensajes recibidos desde otros nodos.
        /// </summary>
        public void ProcessMessage(NetMQMessage message)
        {
            var command = message[0].ConvertToString();

            switch (command)
            {
                case "NodeJoined":
                    Console.WriteLine($"Node joined: {message[1].ConvertToString()}");
                    break;

                case "CustomEvent":
                    HandleCustomEvent(message);
                    break;

                default:
                    Console.WriteLine($"Unknown message command: {command}");
                    break;
            }
        }

        private void PublishMessage(object[] args)
        {
            if (args.Length < 1 || args[0] is not string message)
            {
                Console.WriteLine("Invalid PublishMessage command arguments.");
                return;
            }

            var netMqMessage = new NetMQMessage();
            netMqMessage.Append("CustomEvent");
            netMqMessage.Append(message);
            _transporter.SendMessage(netMqMessage);
        }

        private void GetActiveNodes()
        {
            var activeNodes = _nodeManager.GetActiveNodes();
            Console.WriteLine("Active nodes:");
            foreach (var node in activeNodes)
            {
                Console.WriteLine(node);
            }
        }

        private void HandleCustomEvent(NetMQMessage message)
        {
            // Implementar lógica específica para eventos personalizados
            Console.WriteLine($"Custom event received: {message[1].ConvertToString()}");
        }
    }
}
