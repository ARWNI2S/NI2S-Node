using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting.Startup
{
    /// <summary>
    /// Provides an interface for initializing services and middleware used by a engine.
    /// </summary>
    public interface INodeStartup
    {
        /// <summary>
        /// Register services into the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        IServiceProvider ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Configures the engine.
        /// </summary>
        /// <param name="app">An <see cref="IEngineBuilder"/> for the app to configure.</param>
        void Configure(IEngineBuilder app);
    }
}
