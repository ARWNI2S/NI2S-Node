using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node.Hosting.Infrastructure
{
    /// <summary>
    /// Provides an interface for initializing services and middleware used by an node.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Register services into the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        IServiceProvider ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Configures the node.
        /// </summary>
        /// <param name="app">An <see cref="INodeBuilder"/> for the app to configure.</param>
        void Configure(INodeBuilder app);
    }
}
