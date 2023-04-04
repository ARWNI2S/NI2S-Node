using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Builder;
using NI2S.Node.Hosting.Server;
using NI2S.Node.Hosting.Server.Features;
using NI2S.Node.Dummy;
using NI2S.Node.Clustering;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Engine.Modules;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// The node engine used to configure the message handler pipeline, and cluster routes.
    /// </summary>
    /// <remarks>Was <see cref="NodeEngine"/></remarks>
    public sealed class NodeEngine : IHost, IEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    {
        internal const string GlobalEndpointRouteBuilderKey = "__GlobalEndpointRouteBuilder";

        private readonly IHost _host;
        private readonly List<EndpointDataSource> _dataSources = new();

        internal NodeEngine(IHost host)
        {
            _host = host;
            EngineBuilder = new EngineBuilder(host.Services, ServerFeatures);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName ?? nameof(NodeEngine));

            Properties[GlobalEndpointRouteBuilderKey] = this;
        }

        /// <summary>
        /// The application's configured services.
        /// </summary>
        public IServiceProvider Services => _host.Services;

        /// <summary>
        /// The application's configured <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

        /// <summary>
        /// The application's configured <see cref="INodeHostEnvironment"/>.
        /// </summary>
        public INodeHostEnvironment Environment => _host.Services.GetRequiredService<INodeHostEnvironment>();

        /// <summary>
        /// Allows consumers to be notified of application lifetime events.
        /// </summary>
        public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

        /// <summary>
        /// The default logger for the application.
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

        internal ICollection<EndpointDataSource> DataSources => _dataSources;
        ICollection<EndpointDataSource> IClusterNodeBuilder.DataSources => DataSources;

        internal EngineBuilder EngineBuilder { get; }

        IServiceProvider IClusterNodeBuilder.ServiceProvider => Services;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngine"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NodeEngine"/>.</returns>
        public static NodeEngine Create(string[] args = null) =>
            new NodeEngineBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NodeEngineBuilder"/>.</returns>
        public static NodeEngineBuilder CreateBuilder() =>
            new(new());

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NodeEngineBuilder"/>.</returns>
        public static NodeEngineBuilder CreateBuilder(string[] args) =>
            new(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="NodeEngineOptions"/> to configure the <see cref="NodeEngineBuilder"/>.</param>
        /// <returns>The <see cref="NodeEngineBuilder"/>.</returns>
        public static NodeEngineBuilder CreateBuilder(NodeEngineOptions options) =>
            new(options);

        /// <summary>
        /// Start the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the startup of the <see cref="NodeEngine"/>.
        /// Successful completion indicates the HTTP server is ready to accept new requests.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _host.StartAsync(cancellationToken);

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the shutdown of the <see cref="NodeEngine"/>.
        /// Successful completion indicates that all the HTTP server has stopped.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _host.StopAsync(cancellationToken);

        /// <summary>
        /// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the entire runtime of the <see cref="NodeEngine"/> from startup to shutdown.
        /// </returns>
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

        internal RequestDelegate BuildRequestDelegate() => EngineBuilder.Build();
        RequestDelegate IEngineBuilder.Build() => BuildRequestDelegate();

        // REVIEW: Should this be wrapping another type?
        IEngineBuilder IEngineBuilder.New()
        {
            var newBuilder = EngineBuilder.New();
            // Remove the route builder so branched pipelines have their own routing world
            newBuilder.Properties.Remove(GlobalEndpointRouteBuilderKey);
            return newBuilder;
        }

        /// <summary>
        /// Adds the middleware to the application request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns>An instance of <see cref="IEngineBuilder"/> after the operation has completed.</returns>
        public IEngineBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            EngineBuilder.Use(middleware);
            return this;
        }

        IEngineBuilder IClusterNodeBuilder.CreateEngineBuilder() => ((IEngineBuilder)this).New();

        private void Listen(string url)
        {
            if (url is null)
            {
                return;
            }

            var addresses = (ServerFeatures.Get<IServerAddressesFeature>()?.Addresses) ?? throw new InvalidOperationException($"Changing the URL is not supported because no valid {nameof(IServerAddressesFeature)} was found.");
            if (addresses.IsReadOnly)
            {
                throw new InvalidOperationException($"Changing the URL is not supported because {nameof(IServerAddressesFeature.Addresses)} {nameof(ICollection<string>.IsReadOnly)}.");
            }

            addresses.Clear();
            addresses.Add(url);
        }
    }

}
