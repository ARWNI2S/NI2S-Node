using ARWNI2S.Engine.Clustering.Data;

namespace ARWNI2S.Engine.Clustering
{
    public class ClusterMap
    {
        public List<ClusterNode> OnlineNodes { get; set; }
        public List<ClusterNode> OfflineNodes { get; set; }
        public List<ClusterNode> NodesWithError { get; set; }
        public List<ClusterNode> SpinningUpNodes { get; set; }
        public List<ClusterNode> SpinningDownNodes { get; set; }
        public ClusterStatus ClusterStatus { get; set; }
    }
}