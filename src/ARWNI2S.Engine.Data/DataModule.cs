using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Data
{
    public class DataModule : EngineModule
    {
        public override int Order => 0;

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
