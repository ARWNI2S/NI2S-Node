using ARWNI2S.Engine.Builder;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ARWNI2S.Extensibility
{
    public interface IEngineModule : ILifecycleParticipant<IEngineLifecycle>
    {
        /// <summary>
        /// Add and configure any of the engine services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="engineBuilder">Builder for configuring an application's request pipeline</param>
        void ConfigureEngine(IEngineBuilder engineBuilder);
    }
}
