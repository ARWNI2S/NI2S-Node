using ARWNI2S.Engine.Builder;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine
{
    public interface IEngineModule : ILifecycleParticipant<ILifecycleSubject>
    {
        IFeatureCollection Features { get; }

        void ConfigureServices(IServiceCollection services);
        void Configure(IEngineBuilder engineBuilder);
    }
}
