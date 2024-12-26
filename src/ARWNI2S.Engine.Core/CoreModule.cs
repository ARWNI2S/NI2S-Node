using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine
{
    public class CoreModule : EngineModule
    {
        public override int Order => NI2SLifecycleStage.RuntimeInitialize - 1;

        public override void ConfigureEngine(IEngineBuilder engineBuilder)
        {

        }

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }

        protected override async Task OnStart(CancellationToken token)
        {
            await Task.CompletedTask;
        }

        protected override async Task OnStop(CancellationToken token)
        {
            await Task.CompletedTask;
        }
    }
}
