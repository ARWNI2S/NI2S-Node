// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;
using System;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Allows fine grained configuration of essential MVC services.
    /// </summary>
    public class NI2SCoreBuilder : INI2SCoreBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="MvcCoreBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="manager">The <see cref="ApplicationPartManager"/> of the engine.</param>
        public NI2SCoreBuilder(
            IServiceCollection services,
            IModuleManager manager)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(manager);

            Services = services;
            ModuleManager = manager;
        }

        /// <inheritdoc />
        public IModuleManager ModuleManager { get; }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}
