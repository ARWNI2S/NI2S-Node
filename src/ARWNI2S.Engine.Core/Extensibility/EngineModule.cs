using ARWNI2S.Engine.Builder;
using ARWNI2S.Environment;
using ARWNI2S.Extensibility;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ARWNI2S.Engine.Extensibility
{
    public abstract class EngineModule : EngineModuleBase, IEngineModule
    {
        public abstract string SystemName { get; set; }
        public abstract string FriendlyName { get; set; }

        protected readonly ITypeFinder TypeFinder = Singleton<ITypeFinder>.Instance;

        public override void ConfigureEngine(IEngineBuilder engineBuilder)
        {
            engineBuilder.EngineServices.GetRequiredService<IEngineModuleManager>().Register(this);
        }
    }

    public abstract class EngineModuleBase : IConfigureEngine, ILifecycleParticipant<IEngineLifecycle>
    {
        public virtual int Order => NI2SLifecycleStage.RuntimeInitialize;

        public abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        public abstract void ConfigureEngine(IEngineBuilder engineBuilder);

        public virtual void Participate(IEngineLifecycle lifecycle)
        {
            lifecycle.Subscribe(GetType().Name, Order, OnStart, OnStop);
        }

        protected virtual Task OnStart(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnStop(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}