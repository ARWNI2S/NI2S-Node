using NetMQ;
using NetMQ.Sockets;

namespace ARWNI2S.Cluster.Networking
{
    internal class MessageTransporter
    {
        private readonly PublisherSocket _publisher;
        private readonly SubscriberSocket _subscriber;

        public event Action<NetMQMessage> MessageReceived;

        public MessageTransporter()
        {
            _publisher = new PublisherSocket();
            _subscriber = new SubscriberSocket();
        }

        public void Start()
        {
            _subscriber.Subscribe("");
            _subscriber.ReceiveReady += OnMessageReceived;

            NetMQPoller poller = [_subscriber];
            poller.RunAsync();
        }

        public void SendMessage(NetMQMessage message)
        {
            _publisher.SendMultipartMessage(message);
        }

        private void OnMessageReceived(object sender, NetMQSocketEventArgs e)
        {
            var message = _subscriber.ReceiveMultipartMessage();
            MessageReceived?.Invoke(message);
        }
    }
}
