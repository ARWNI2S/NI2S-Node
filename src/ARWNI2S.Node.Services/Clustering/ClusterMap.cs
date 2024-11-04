using ARWNI2S.Node.Core.Entities.Clustering;

namespace ARWNI2S.Node.Services.Clustering
{
    public class ClusterMap
    {
        public List<NI2SNode> OnlineNodes { get; internal set; }
        public List<NI2SNode> OfflineNodes { get; internal set; }
        public List<NI2SNode> NodesWithError { get; internal set; }
        public List<NI2SNode> SpinningUpNodes { get; internal set; }
        public List<NI2SNode> SpinningDownNodes { get; internal set; }
        public ClusterStatus ClusterStatus { get; internal set; }
    }
}