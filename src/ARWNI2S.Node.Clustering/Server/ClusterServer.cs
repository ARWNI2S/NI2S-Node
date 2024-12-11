using ARWNI2S.Clustering.Data;
using ARWNI2S.Clustering.Nodes.Features;
using ARWNI2S.Configuration;
using ARWNI2S.Engine.Features;

namespace ARWNI2S.Clustering.Server
{
    internal class ClusterServer : IClusterServer
    {
        private readonly NI2SSettings _settings;

        public NodeFeatures Features { get; }

        public Guid NodeId => Node.NodeId;
        public NI2SNode Node { get; protected set; }

        IFeatureCollection IClusterServer.Features => Features;
        INiisNode IClusterServer.Node => Node;

        public ClusterServer(NI2SSettings settings)
        {
            _settings = settings;
        }
    }
}
