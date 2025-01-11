using ARWNI2S.Configuration;

namespace ARWNI2S.Cluster.Entities
{
    public class ClusterSettings : ISettings
    {
        public bool IgnoreNodeLimitations { get; internal set; }
    }
}
