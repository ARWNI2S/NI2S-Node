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
