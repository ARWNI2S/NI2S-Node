// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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