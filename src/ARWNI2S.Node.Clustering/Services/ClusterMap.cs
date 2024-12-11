using ARWNI2S.Clustering.Data;

namespace ARWNI2S.Clustering.Services
{
    public class ClusterMap
    {
        public List<NI2SNode> OnlineNodes { get; set; }
        public List<NI2SNode> OfflineNodes { get; set; }
        public List<NI2SNode> NodesWithError { get; set; }
        public List<NI2SNode> SpinningUpNodes { get; set; }
        public List<NI2SNode> SpinningDownNodes { get; set; }
        public ClusterStatus ClusterStatus { get; set; }
    }
}