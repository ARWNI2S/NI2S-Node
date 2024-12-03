using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring services and modules on engine startup
    /// </summary>
    public partial interface INiisStartup
    {
        /// <summary>
        /// Add and configure any of the engine services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the engine</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Configure the using of added modules
        /// </summary>
        /// <param name="builder">Builder for configuring a engine's update pipeline</param>
        void Configure(IEngineBuilder builder);

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        int Order { get; }
    }
}
