using Microsoft.Extensions.Options;

namespace ARWNI2S.Cluster.Hosting
{
    internal class ClusterNodeOptionsSetup : IConfigureOptions<ClusterNodeOptions>
    {
        public void Configure(ClusterNodeOptions options)
        {
        }
    }
}