// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Cluster.Networking
{
    internal class NI2SNetworkModule : FrameworkModule
    {
        //public override int Order => ClusterLifecycleStage.NodeInitialize;

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
