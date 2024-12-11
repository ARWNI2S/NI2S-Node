using ARWNI2S.Clustering.Configuration;
using ARWNI2S.Clustering.Data;
using ARWNI2S.Engine.Features;

namespace ARWNI2S.Clustering
{
    internal class ClusterServer : IClusterServer
    {
        private readonly ClusterServerOptions _options;

        public NodeFeatures Features { get; }

        public Guid NodeId => Node.NodeId;
        public NI2SNode Node { get; protected set; }

        IFeatureCollection IClusterServer.Features => Features;
        INiisNode IClusterServer.Node => Node;

        public ClusterServer(ClusterServerOptions options)
        {
            _options = options;
        }
    }
}
