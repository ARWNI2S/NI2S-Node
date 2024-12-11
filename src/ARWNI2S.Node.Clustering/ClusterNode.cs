using ARWNI2S.Engine;

namespace ARWNI2S.Clustering
{
    internal class ClusterNode : INiisRelay
    {
        private readonly INiisNode _nodeData;

        public NodeFeatures Features { get; }

        IFeatureCollection INiisRelay.Features => Features;

        Guid INiisNode.NodeId => _nodeData.NodeId;

        int IDataEntity.Id
        {
            get => _nodeData.Id;
            set => _nodeData.Id = value;
        }
    }
}
