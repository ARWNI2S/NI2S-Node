using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Cluster.Connection
{
    internal sealed class ConnectionReference
    {
        private readonly long _id;
        private readonly WeakReference<ClusterNodeConnection> _weakReference;
        private readonly TransportConnectionManager _transportConnectionManager;

        public ConnectionReference(long id, ClusterNodeConnection connection, TransportConnectionManager transportConnectionManager)
        {
            _id = id;

            _weakReference = new WeakReference<ClusterNodeConnection>(connection);
            ConnectionId = connection.TransportConnection.ConnectionId;

            _transportConnectionManager = transportConnectionManager;
        }

        public string ConnectionId { get; }

        public bool TryGetConnection([NotNullWhen(true)] out ClusterNodeConnection connection)
        {
            return _weakReference.TryGetTarget(out connection);
        }

        public void StopTransportTracking()
        {
            _transportConnectionManager.StopTracking(_id);
        }
    }
}