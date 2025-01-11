// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Cluster.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Cluster
{
    internal class NI2SClusterModule : ClusterModule
    {
        //public override int Order => NI2SLifecycleStage.BecomeActive;

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
