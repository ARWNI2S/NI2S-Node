using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Clustering;
using NI2S.Node.Configuration;
using NI2S.Node.Dummy;
using NI2S.Node.Engine.Modules;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// The NI2S node used to configure the engine, and clustering.
    /// </summary>
    public sealed class NI2SNode : IHost, INodeBuilder, INodeClusterBuilder, IAsyncDisposable
    {
        internal const string GlobalNodeClusterBuilderKey = "__GlobalNodeClusterBuilder";

        private readonly IHost _host;
        private readonly List<DummyDataSource> _dataSources = new();

        #region Properties

        /// <summary>
        /// The NI2S node's configured services.
        /// </summary>
        public IServiceProvider Services => _host.Services;

        /// <summary>
        /// The NI2S node's configured <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

        /// <summary>
        /// The NI2S node's configured <see cref="INodeHostEnvironment"/>.
        /// </summary>
        public INodeHostEnvironment Environment => _host.Services.GetRequiredService<INodeHostEnvironment>();

        /// <summary>
        /// Allows consumers to be notified of NI2S node lifetime events.
        /// </summary>
        public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

        /// <summary>
        /// The default logger for the NI2S node.
        /// </summary>
        public ILogger Logger { get; }

        // <summary>
        // The list of URLs that the HTTP server is bound to.
        // </summary>
        //public ICollection<string> Urls => ServerFeatures.GetRequiredFeature<IServerAddressesFeature>().Addresses;

        #endregion

        internal NI2SNode(IHost host)
        {
            _host = host;
            // TODO rewite ServerFeatures into ApplicationModules 
            NodeBuilder = new NodeBuilder(host.Services, ServerFeatures);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName ?? nameof(NI2SNode));

            Properties[GlobalNodeClusterBuilderKey] = this;
        }

        IServiceProvider INodeBuilder.ApplicationServices
        {
            get => NodeBuilder.ApplicationServices;
            set => NodeBuilder.ApplicationServices = value;
        }

        internal IDummyFeatureCollection ServerFeatures => _host.Services.GetRequiredService<IDummyServer>().Features;
        IDummyFeatureCollection INodeBuilder.ServerFeatures => ServerFeatures;

        internal IDictionary<string, object> Properties => NodeBuilder.Properties;
        IDictionary<string, object> INodeBuilder.Properties => Properties;

        internal ICollection<DummyDataSource> DataSources => _dataSources;
        ICollection<DummyDataSource> INodeClusterBuilder.DataSources => DataSources;

        internal NodeBuilder NodeBuilder { get; }

        IServiceProvider INodeClusterBuilder.ServiceProvider => Services;

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNode"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NI2SNode"/>.</returns>
        public static NI2SNode Create(string[] args = null) =>
            new NI2SNodeBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateBuilder() =>
            new(new());

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateBuilder(string[] args) =>
            new(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="NI2SNodeOptions"/> to configure the <see cref="NI2SNodeBuilder"/>.</param>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateBuilder(NI2SNodeOptions options) =>
            new(options);

        /// <summary>
        /// Start the NI2S node.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the startup of the <see cref="NI2SNode"/>.
        /// Successful completion indicates the HTTP server is ready to accept new requests.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _host.StartAsync(cancellationToken);

        /// <summary>
        /// Shuts down the NI2S node.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the shutdown of the <see cref="NI2SNode"/>.
        /// Successful completion indicates that all the HTTP server has stopped.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _host.StopAsync(cancellationToken);

        /// <summary>
        /// Runs an NI2S node and returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the entire runtime of the <see cref="NI2SNode"/> from startup to shutdown.
        /// </returns>
        public Task RunAsync(string url = null)
        {
            Listen(url);
            return HostingAbstractionsHostExtensions.RunAsync(this);
        }

        /// <summary>
        /// Runs an NI2S node and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        public void Run(string url = null)
        {
            Listen(url);
            HostingAbstractionsHostExtensions.Run(this);
        }

        /// <summary>
        /// Disposes the NI2S node.
        /// </summary>
        void IDisposable.Dispose() => _host.Dispose();

        /// <summary>
        /// Disposes the NI2S node.
        /// </summary>
        public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

        internal MessageDelegate BuildMessageDelegate() => NodeBuilder.Build();
        MessageDelegate INodeBuilder.Build() => BuildMessageDelegate();

        // REVIEW: Should this be wrapping another type?
        INodeBuilder INodeBuilder.New()
        {
            var newBuilder = NodeBuilder.New();
            // Remove the route builder so branched pipelines have their own routing world
            newBuilder.Properties.Remove(GlobalNodeClusterBuilderKey);
            return newBuilder;
        }

        /// <summary>
        /// Adds the middleware to the NI2S node request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns>An instance of <see cref="INodeBuilder"/> after the operation has completed.</returns>
        public INodeBuilder Use(Func<MessageDelegate, MessageDelegate> middleware)
        {
            NodeBuilder.Use(middleware);
            return this;
        }

        INodeBuilder INodeClusterBuilder.CreateNodeBuilder() => ((INodeBuilder)this).New();

        private void Listen(string url)
        {
            if (url is null)
            {
                return;
            }

            var addresses = (ServerFeatures.Get<IDummyAddressesFeature>()?.Addresses) ?? throw new InvalidOperationException($"Changing the URL is not supported because no valid {nameof(IDummyAddressesFeature)} was found.");
            if (addresses.IsReadOnly)
            {
                throw new InvalidOperationException($"Changing the URL is not supported because {nameof(IDummyAddressesFeature.Addresses)} {nameof(ICollection<string>.IsReadOnly)}.");
            }

            addresses.Clear();
            addresses.Add(url);
        }
    }
}
