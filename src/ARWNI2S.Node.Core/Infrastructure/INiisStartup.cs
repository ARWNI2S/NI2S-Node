using ARWNI2S.Node.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring services on application startup
    /// </summary>
    public partial interface INiisStartup
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
        /// <param name="engineBuilder">Builder for configuring the engine</param>
        void Configure(IEngineBuilder engineBuilder);

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        int Order { get; }
    }
}
