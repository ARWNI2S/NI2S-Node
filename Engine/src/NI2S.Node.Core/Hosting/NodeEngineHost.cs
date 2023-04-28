// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Data;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Used to configure the NI2S node engine host.
    /// </summary>
    public sealed class NodeEngineHost : IHost, IEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    {
        internal const string GlobalClusterNodeBuilderKey = "__GlobalClusterNodeBuilder";

        private readonly IHost _host;

        /* 003.4 - ... -> .Build() -> Host.ApplyServiceProviderFactory(...) -> new NodeEngineHost(...) */
        internal NodeEngineHost(IHost host)
        {
            _host = host;
            EngineBuilder = new EngineBuilder(host.Services, EngineModules);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName ?? nameof(NodeEngineHost));

            Properties[GlobalClusterNodeBuilderKey] = this;
        }

        /// <summary>
        /// The node engine's configured services.
        /// </summary>
        public IServiceProvider Services => _host.Services;

        /// <summary>
        /// The node engine's configured <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

        public List<IDataSource> DataSources => _host.Services.GetRequiredService<IClusterManager>().GetDataSources();
     
        /// <summary>
        /// The node engine's configured <see cref="INodeHostEnvironment"/>.
        /// </summary>
        public INodeHostEnvironment Environment => _host.Services.GetRequiredService<INodeHostEnvironment>();

        /// <summary>
        /// Allows consumers to be notified of application lifetime events.
        /// </summary>
        public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

        /// <summary>
        /// The default logger for the node.
        /// </summary>
        public ILogger Logger { get; }

        IServiceProvider IEngineBuilder.EngineServices
        {
            get => EngineBuilder.EngineServices;
            set => EngineBuilder.EngineServices = value;
        }

        internal IModuleCollection EngineModules => _host.Services.GetRequiredService<IEngine>().Modules;

        IModuleCollection IEngineBuilder.EngineModules => EngineModules;

        internal IDictionary<string, object> Properties => EngineBuilder.Properties;
        IDictionary<string, object> IEngineBuilder.Properties => Properties;

        internal EngineBuilder EngineBuilder { get; }

        IServiceProvider IClusterNodeBuilder.ServiceProvider => Services;

        ICollection<IDataSource> IClusterNodeBuilder.DataSources => DataSources;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NodeEngineHostBuilder"/>.</returns>
        public static NodeEngineHostBuilder CreateBuilder(string[] args) =>
            new(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="NodeEngineHostOptions"/> to configure the <see cref="NodeEngineHostBuilder"/>.</param>
        /// <returns>The <see cref="NodeEngineHostBuilder"/>.</returns>
        public static NodeEngineHostBuilder CreateBuilder(NodeEngineHostOptions options) =>
            new(options);

        /// <summary>
        /// Start the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the startup of the <see cref="NodeEngineHost"/>.
        /// Successful completion indicates the HTTP server is ready to accept new requests.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _host.StartAsync(cancellationToken);

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the shutdown of the <see cref="NodeEngineHost"/>.
        /// Successful completion indicates that all the HTTP server has stopped.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _host.StopAsync(cancellationToken);

        /// <summary>
        /// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the entire runtime of the <see cref="NodeEngineHost"/> from startup to shutdown.
        /// </returns>
        /* 006 - ... -> nodeEngine.RunAsync() */
        public Task RunAsync(string url = null)
        {
            Listen(url);
            return HostingAbstractionsHostExtensions.RunAsync(this);
        }

        /// <summary>
        /// Runs an application and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        public void Run(string url = null)
        {
            Listen(url);
            HostingAbstractionsHostExtensions.Run(this);
        }

        /// <summary>
        /// Disposes the application.
        /// </summary>
        void IDisposable.Dispose() => _host.Dispose();

        /// <summary>
        /// Disposes the application.
        /// </summary>
        public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

        internal IEngine BuildNodeEngine() => EngineBuilder.Build();
        IEngine IEngineBuilder.Build() => BuildNodeEngine();

        // REVIEW: Should this be wrapping another type?
        IEngineBuilder IEngineBuilder.New()
        {
            var newBuilder = EngineBuilder.New();
            // Remove the route builder so branched pipelines have their own routing world
            newBuilder.Properties.Remove(GlobalClusterNodeBuilderKey);
            return newBuilder;
        }

        IEngineBuilder IClusterNodeBuilder.CreateEngineBuilder() => ((IEngineBuilder)this).New();

        /* 006.1 - ... -> nodeEngine.RunAsync() -> Listen(...) */
        private void Listen(string url)
        {
            if (url is null)
            {
                return;
            }

            // TODO: Do cluster listen update.
            //var addresses = (EngineModules.Get<IServerAddressesModule>()?.Addresses) ?? throw new InvalidOperationException($"Changing the URL is not supported because no valid {nameof(IServerAddressesModule)} was found.");
            //if (addresses.IsReadOnly)
            //{
            //    throw new InvalidOperationException($"Changing the URL is not supported because {nameof(IServerAddressesModule.Addresses)} {nameof(ICollection<string>.IsReadOnly)}.");
            //}

            //addresses.Clear();
            //addresses.Add(url);
        }
    }
}
