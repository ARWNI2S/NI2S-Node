using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;

namespace NI2S.Node.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring services and middleware on node startup
    /// </summary>
    public partial interface INodeStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the node</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="node">Builder for configuring an node's request pipeline</param>
        void Configure(INodeBuilder node);

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        int Order { get; }
    }
}
