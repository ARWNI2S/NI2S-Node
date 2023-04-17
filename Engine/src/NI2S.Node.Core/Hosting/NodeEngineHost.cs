// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
    public sealed class NodeEngineHost : IHost, INodeEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    {
        internal const string GlobalClusterNodeBuilderKey = "__GlobalClusterNodeBuilder";

        private readonly IHost _host;

        /* 003.4 - ... -> .Build() -> Host.ApplyServiceProviderFactory(...) -> new NodeEngineHost(...) */
        internal NodeEngineHost(IHost host)
        {
            _host = host;
            EngineBuilder = new NodeEngineBuilder(host.Services, EngineModules);
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

        IServiceProvider INodeEngineBuilder.EngineServices
        {
            get => EngineBuilder.EngineServices;
            set => EngineBuilder.EngineServices = value;
        }

        internal IModuleCollection EngineModules => _host.Services.GetRequiredService<INodeEngine>().Modules;

        IModuleCollection INodeEngineBuilder.EngineModules => EngineModules;

        internal IDictionary<string, object> Properties => EngineBuilder.Properties;
        IDictionary<string, object> INodeEngineBuilder.Properties => Properties;

        internal NodeEngineBuilder EngineBuilder { get; }

        IServiceProvider IClusterNodeBuilder.ServiceProvider => Services;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineHost"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NodeEngineHost"/>.</returns>
        public static NodeEngineHost Create(string[] args = null) =>
            new NodeEngineHostBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NodeEngineHostBuilder"/>.</returns>
        public static NodeEngineHostBuilder CreateBuilder() =>
            new(new());

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

        internal INodeEngine BuildNodeEngine() => EngineBuilder.Build();
        INodeEngine INodeEngineBuilder.Build() => BuildNodeEngine();

        // REVIEW: Should this be wrapping another type?
        INodeEngineBuilder INodeEngineBuilder.New()
        {
            var newBuilder = EngineBuilder.New();
            // Remove the route builder so branched pipelines have their own routing world
            newBuilder.Properties.Remove(GlobalClusterNodeBuilderKey);
            return newBuilder;
        }

        /// <summary>
        /// Adds the middleware to the application request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns>An instance of <see cref="INodeEngineBuilder"/> after the operation has completed.</returns>
        //public IEngineBuilder Use(Func<INodeEngine, INodeEngine> middleware)
        //{
        //    EngineBuilder.Use(middleware);
        //    return this;
        //}

        INodeEngineBuilder IClusterNodeBuilder.CreateEngineBuilder() => ((INodeEngineBuilder)this).New();

        /* 006.1 - ... -> nodeEngine.RunAsync() -> Listen(...) */
        private void Listen(string url)
        {
            //if (url is null)
            //{
            //    return;
            //}

            //var addresses = ServerModules.Get<IServerAddressesModule>()?.Addresses;
            //if (addresses is null)
            //{
            //    throw new InvalidOperationException($"Changing the URL is not supported because no valid {nameof(IServerAddressesModule)} was found.");
            //}
            //if (addresses.IsReadOnly)
            //{
            //    throw new InvalidOperationException($"Changing the URL is not supported because {nameof(IServerAddressesModule.Addresses)} {nameof(ICollection<string>.IsReadOnly)}.");
            //}

            //addresses.Clear();
            //addresses.Add(url);
        }
    }
}
