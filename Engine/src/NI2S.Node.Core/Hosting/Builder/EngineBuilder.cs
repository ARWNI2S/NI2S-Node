// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Engine;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Default implementation for <see cref="IEngineBuilder"/>.
    /// </summary>
    internal class EngineBuilder : IEngineBuilder
    {
        private const string EngineModulesKey = "engine.Modules";
        private const string EngineServicesKey = "engine.Services";

        /// <summary>
        /// Initializes a new instance of <see cref="EngineBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        public EngineBuilder(IServiceProvider serviceProvider) : this(serviceProvider, new ModuleCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EngineBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        /// <param name="modules">The engine instance that hosts the modules.</param>
        public EngineBuilder(IServiceProvider serviceProvider, IModuleCollection modules)
        {
            Properties = new Dictionary<string, object>(StringComparer.Ordinal);
            EngineServices = serviceProvider;

            SetProperty(EngineModulesKey, modules);
        }

        private EngineBuilder(EngineBuilder builder)
        {
            Properties = new CopyOnWriteDictionary<string, object>(builder.Properties, StringComparer.Ordinal);
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> for application services.
        /// </summary>
        public IServiceProvider EngineServices
        {
            get
            {
                return GetProperty<IServiceProvider>(EngineServicesKey)!;
            }
            set
            {
                SetProperty(EngineServicesKey, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="IModuleCollection"/> for server modules.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a server wasn't specified for the application builder.
        /// </remarks>
        public IModuleCollection EngineModules
        {
            get
            {
                return GetProperty<IModuleCollection>(EngineModulesKey)!;
            }
        }

        /// <summary>
        /// Gets a set of properties for <see cref="EngineBuilder"/>.
        /// </summary>
        public IDictionary<string, object> Properties { get; }

        private T GetProperty<T>(string key)
        {
            return Properties.TryGetValue(key, out var value) ? (T)value : default;
        }

        private void SetProperty<T>(string key, T value)
        {
            Properties[key] = value;
        }

        /// <summary>
        /// Creates a copy of this application builder.
        /// <para>
        /// The created clone has the same properties as the current instance, but does not copy
        /// the request pipeline.
        /// </para>
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public IEngineBuilder New()
        {
            return new EngineBuilder(this);
        }

        /// <summary>
        /// Produces a <see cref="IEngine"/> that executes added middlewares.
        /// </summary>
        /// <returns>The <see cref="IEngine"/>.</returns>
        public IEngine Build()
        {
            IEngine engine = EngineServices.GetRequiredService<IEngine>() ?? throw new InvalidOperationException();

            //TODO: BUILD NODE ENGINE

            return engine;
        }
    }
}
