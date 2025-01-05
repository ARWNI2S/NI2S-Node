using ARWNI2S.Configuration;

namespace ARWNI2S.Cluster.Configuration
{
    public class ClusterConfig : IConfig
    {
        public string ClusterName { get; set; }
        public string ClusterId { get; set; }
    }
}
