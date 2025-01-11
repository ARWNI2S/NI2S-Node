using Microsoft.Extensions.Options;

namespace ARWNI2S.Cluster.Configuration
{
    internal class ClusterNodeOptionsSetup : IConfigureOptions<ClusterNodeOptions>
    {
        private readonly IServiceProvider _services;

        public ClusterNodeOptionsSetup(IServiceProvider services)
        {
            _services = services;
        }

        public void Configure(ClusterNodeOptions options)
        {
            options.EngineServices = _services;
        }
    }
}