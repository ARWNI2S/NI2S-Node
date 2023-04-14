// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node.Engine
{
    /// <summary>
    /// Used for initializing services and modules used by a node engine.
    /// </summary>
    internal class NodeEngineStartup : StartupBase<IServiceCollection>
    {
        private readonly Action<INodeEngineBuilder> _configureApp;

        /// <summary>
        /// Creates a new <see cref="NodeEngineStartup" /> instance.
        /// </summary>
        /// <param name="factory">A factory for creating <see cref="IServiceProvider"/> instances.</param>
        /// <param name="configureApp">An <see cref="Action"/> for configuring the application.</param>
        public NodeEngineStartup(IServiceProviderFactory<IServiceCollection> factory, Action<INodeEngineBuilder> configureApp) : base(factory)
        {
            _configureApp = configureApp;
        }

        /// <summary>
        /// Configures the <see cref="INodeEngineBuilder"/> with the initialized <see cref="Action"/>.
        /// </summary>
        /// <param name="app">The <see cref="INodeEngineBuilder"/>.</param>
        public override void Configure(INodeEngineBuilder app) => _configureApp(app);
    }
}
