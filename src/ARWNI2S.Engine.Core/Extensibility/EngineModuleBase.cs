using ARWNI2S.Engine.Builder;
using ARWNI2S.Extensibility;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ARWNI2S.Engine.Extensibility
{
    public abstract class EngineModule : EngineModuleBase, IEngineModule
    {
        public virtual int Order => NI2SLifecycleStage.RuntimeInitialize;

        public abstract void ConfigureEngine(IEngineBuilder engineBuilder);
        public abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        public override void Participate(IEngineLifecycle lifecycle)
        {
            lifecycle.Subscribe(GetType().Name, Order, OnStart, OnStop);
        }
    }

    public abstract class EngineModuleBase : ILifecycleParticipant<IEngineLifecycle>
    {
        public abstract void Participate(IEngineLifecycle lifecycle);

        protected abstract Task OnStop(CancellationToken token);
        protected abstract Task OnStart(CancellationToken token);
    }
}