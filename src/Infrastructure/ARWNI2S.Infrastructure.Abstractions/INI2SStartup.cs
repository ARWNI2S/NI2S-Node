using ARWNI2S.Infrastructure.Engine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring services and middleware on application startup
    /// </summary>
    public partial interface INI2SStartup
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
        void Configure(IEngineBuilder engineBuilder); //INI2SHostBuilder application);

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        int Order { get; }
    }
}
