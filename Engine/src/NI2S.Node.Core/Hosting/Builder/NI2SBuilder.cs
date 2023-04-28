// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;
using System;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Allows fine grained configuration of MVC services.
    /// </summary>
    internal sealed class NI2SBuilder : INI2SBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="MvcBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="manager">The <see cref="IModuleManager"/> of the engine.</param>
        public NI2SBuilder(IServiceCollection services, IModuleManager manager)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(manager);

            Services = services;
            ModuleManager = manager;
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }

        /// <inheritdoc />
        public IModuleManager ModuleManager { get; }
    }
}
