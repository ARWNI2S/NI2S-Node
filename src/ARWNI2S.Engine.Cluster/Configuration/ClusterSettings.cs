using ARWNI2S.Framework.Configuration;

namespace ARWNI2S.Cluster.Configuration
{
    public class ClusterSettings : ISettings
    {
        public bool IgnoreNodeLimitations { get; internal set; }
    }
}
