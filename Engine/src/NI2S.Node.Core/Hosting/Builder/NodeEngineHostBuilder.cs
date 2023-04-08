// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A builder for NI2S node engine hosting and services.
    /// </summary>
    public sealed class NodeEngineHostBuilder
    {
        private readonly HostApplicationBuilder _hostEngineBuilder;

        private NodeEngineHost _builtNodeEngineHost;

        internal NodeEngineHostBuilder(string[] args)
        {
            _hostEngineBuilder = Host.CreateApplicationBuilder(args);

            // Set NodeRootPath if necessary
            Configuration.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>(NodeHostDefaults.NodeRootKey, NodeHostDefaults.NodeRootPath),
            });

            Environment = new HostingEnvironment(_hostEngineBuilder.Environment, _hostEngineBuilder.Configuration);
        }

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public ConfigurationManager Configuration => _hostEngineBuilder.Configuration;

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services => _hostEngineBuilder.Services;

        /// <summary>
        /// Provides information about the hosting environment an application is running in.
        /// </summary>
        public INodeHostEnvironment Environment { get; }

        /// <summary>
        /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging => _hostEngineBuilder.Logging;


        /// <summary>
        /// Builds the <see cref="NodeEngineHost"/>.
        /// </summary>
        /// <returns>A configured <see cref="NodeEngineHost"/>.</returns>
        public NodeEngineHost Build()
        {
            // ConfigureContainer callbacks run after ConfigureServices callbacks including the one that adds GenericNodeHostService by default.
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            _builtNodeEngineHost = new NodeEngineHost(_hostEngineBuilder.Build());
            return _builtNodeEngineHost;
        }

    }
}
