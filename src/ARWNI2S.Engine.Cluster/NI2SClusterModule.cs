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
