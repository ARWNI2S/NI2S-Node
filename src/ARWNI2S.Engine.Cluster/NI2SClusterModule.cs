using ARWNI2S.Engine.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Cluster
{
    public class NI2SClusterModule : EngineModule
    {
        public override int Order => 0;

        public override string SystemName { get; set; }
        public override string FriendlyName { get; set; }

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
