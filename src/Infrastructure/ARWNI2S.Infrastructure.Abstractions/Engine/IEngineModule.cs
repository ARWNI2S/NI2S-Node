using ARWNI2S.Infrastructure.Engine.Builder;
using ARWNI2S.Infrastructure.Lifecycle;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Infrastructure.Engine
{
    public interface IEngineModule : ILifecycleParticipant<ILifecycleSubject>
    {
        IFeatureCollection Features { get; }

        void ConfigureServices(IServiceCollection services);
        void Configure(IEngineBuilder engineBuilder);
    }
}
