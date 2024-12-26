using ARWNI2S.Engine.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Cluster
{
    public class NI2SClusterModule : EngineModule
    {
        public override int Order => 0;

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
