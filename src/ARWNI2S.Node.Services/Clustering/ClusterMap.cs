using ARWNI2S.Node.Core.Entities.Clustering;

namespace ARWNI2S.Node.Services.Clustering
{
    public class ClusterMap
    {
        public List<ClusterNode> OnlineNodes { get; internal set; }
        public List<ClusterNode> OfflineNodes { get; internal set; }
        public List<ClusterNode> NodesWithError { get; internal set; }
        public List<ClusterNode> SpinningUpNodes { get; internal set; }
        public List<ClusterNode> SpinningDownNodes { get; internal set; }
        public ClusterStatus ClusterStatus { get; internal set; }
    }
}