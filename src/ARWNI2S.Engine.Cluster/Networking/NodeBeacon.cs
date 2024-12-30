using NetMQ;

namespace ARWNI2S.Cluster.Networking
{
    internal class NodeBeacon
    {
        private NetMQBeacon _beacon;

        public NodeBeacon()
        {
            _beacon = new NetMQBeacon();


        }
    }
}
