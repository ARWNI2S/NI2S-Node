using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine
{
    public class CoreModule : EngineModule
    {
        public override int Order => NI2SLifecycleStage.RuntimeInitialize - 1;

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
