// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node
{
    /// <summary>
    /// Provides an interface for initializing services and middleware used by an engine.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Register services into the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        IServiceProvider ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Configures the engine.
        /// </summary>
        /// <param name="engine">An <see cref="IEngineBuilder"/> for the engine to configure.</param>
        void Configure(IEngineBuilder engine);
    }
}