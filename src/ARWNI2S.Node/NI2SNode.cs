using ARWNI2S.Cluster;
using ARWNI2S.Cluster.Builder;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Extensibility;
using ARWNI2S.Hosting;
using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ARWNI2S.Node
{
    [DebuggerDisplay("{DebuggerToString(),nq}")]
    [DebuggerTypeProxy(typeof(NI2SNodeDebugView))]
    public sealed class NI2SNode : IHost, IEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    {
        private readonly IHost _host;
        private readonly List<IModuleDataSource> _dataSources = [];

        #region Properties

        /// <summary>
        /// The application's configured <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

        /// <summary>
        /// The application's configured <see cref="INiisHostEnvironment"/>.
        /// </summary>
        public INiisHostEnvironment Environment => _host.Services.GetRequiredService<INiisHostEnvironment>();

        /// <summary>
        /// Allows consumers to be notified of application lifetime events.
        /// </summary>
        public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

        /// <summary>
        /// The default logger for the application.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// The application's configured services.
        /// </summary>
        public IServiceProvider Services => _host.Services;

        #endregion

        internal EngineBuilder EngineBuilder { get; }
        internal IModuleCollection NodeModules => _host.Services.GetRequiredService<IClusterNode>().Modules;
        internal IDictionary<string, object> Properties => EngineBuilder.Properties;
        internal ICollection<IModuleDataSource> DataSources => _dataSources;

        internal NI2SNode(IHost host)
        {
            _host = host;
            EngineBuilder = new EngineBuilder(host.Services, NodeModules);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName ?? nameof(NI2SNode));

            Properties[NI2SHostingDefaults.GlobalNodeBuilderKey] = this;
        }

        internal INiisEngine BuildNodeEngine() => EngineBuilder.Build();

        #region Methods

        /// <summary>
        /// Start the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the startup of the <see cref="NI2SNode"/>.
        /// Successful completion indicates the HTTP server is ready to accept new requests.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _host.StartAsync(cancellationToken);

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the shutdown of the <see cref="NI2SNode"/>.
        /// Successful completion indicates that all the HTTP server has stopped.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _host.StopAsync(cancellationToken);

        /// <summary>
        /// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the entire runtime of the <see cref="NI2SNode"/> from startup to shutdown.
        /// </returns>
        public async Task RunAsync()
        {
            //HACK await StartEngineAsync();
            await HostingAbstractionsHostExtensions.RunAsync(this);
        }

        /// <summary>
        /// Runs an application and block the calling thread until host shutdown.
        /// </summary>
        public void Run()
        {
            //HACK StartEngine();
            HostingAbstractionsHostExtensions.Run(this);
        }

        #endregion

        #region Utilities

        // NI2S engine is running if the engine has been started and hasn't been stopped.
        private bool IsRunning => Lifetime.ApplicationStarted.IsCancellationRequested && !Lifetime.ApplicationStopped.IsCancellationRequested;

        #endregion

        #region Statics 

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNode"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NI2SNode"/>.</returns>
        public static NI2SNode Create(string[] args = null) =>
            new NodeHostBuilder(args).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NodeHostBuilder"/>.</returns>
        public static NodeHostBuilder CreateBuilder() =>
            new();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NodeHostBuilder"/>.</returns>
        public static NodeHostBuilder CreateBuilder(string[] args) =>
            new(args);

        #endregion

        #region IEngineBuilder 

        IServiceProvider IEngineBuilder.EngineServices
        {
            get => EngineBuilder.EngineServices;
            set => EngineBuilder.EngineServices = value;
        }

        IModuleCollection IEngineBuilder.NodeModules => NodeModules;

        IDictionary<string, object> IEngineBuilder.Properties => Properties;

        INiisEngine IEngineBuilder.Build() => BuildNodeEngine();

        IEngineBuilder IEngineBuilder.New()
        {
            var newBuilder = EngineBuilder.New();
            // Remove the global engine builder so branched flows have their own discovery world
            newBuilder.Properties.Remove(NI2SHostingDefaults.GlobalNodeBuilderKey);
            return newBuilder;
        }

        #endregion

        #region IClusterNodeBuilder

        IEngineBuilder IClusterNodeBuilder.CreateEngineBuilder() => ((IEngineBuilder)this).New();

        ICollection<IModuleDataSource> IClusterNodeBuilder.DataSources => DataSources;

        IServiceProvider IClusterNodeBuilder.ServiceProvider => Services;

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes the application.
        /// </summary>
        void IDisposable.Dispose() => _host.Dispose();

        /// <summary>
        /// Disposes the application.
        /// </summary>
        public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

        #endregion

        internal sealed class NI2SNodeDebugView(NI2SNode niisNode)
        {
            private readonly NI2SNode _niisNode = niisNode;

            public IServiceProvider Services => _niisNode.Services;
            public IConfiguration Configuration => _niisNode.Configuration;
            public INiisHostEnvironment Environment => _niisNode.Environment;
            public IHostApplicationLifetime Lifetime => _niisNode.Lifetime;
            public ILogger Logger => _niisNode.Logger;

            public bool IsRunning => _niisNode.IsRunning;
            public IList<string> Modules
            {
                get
                {
                    if (_niisNode.Properties.TryGetValue(NI2SHostingDefaults.ModuleDescriptionsKey, out var value) &&
                        value is IList<string> descriptions)
                    {
                        return descriptions;
                    }

                    throw new NotSupportedException("Unable to get configured middleware.");
                }
            }
        }
    }
}
