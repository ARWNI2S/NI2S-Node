using ARWNI2S.Hosting.Builder;
using ARWNI2S.Hosting.Configuration;
using ARWNI2S.Hosting.LocalAssets;

namespace ARWNI2S.Hosting
{
    [DebuggerDisplay("{DebuggerToString(),nq}")]
    [DebuggerTypeProxy(typeof(NI2SHostDebugView))]
    public sealed class NI2SHost : IHost, IEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    {
        internal const string GlobalNodeBuilderKey = "__GlobalNodeBuilder";

        private readonly IHost _host;

        internal NI2SHost(IHost host)
        {
            _host = host;
            EngineBuilder = new EngineBuilder(host.Services, NodeFeatures);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName ?? nameof(NI2SHost));

            Properties[GlobalNodeBuilderKey] = this;
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

        IServiceProvider IEngineBuilder.EngineServices
        {
            get => EngineBuilder.EngineServices;
            set => EngineBuilder.EngineServices = value;
        }

        internal IFeatureCollection NodeFeatures => _host.Services.GetRequiredService<IClusterServer>().Features;
        IFeatureCollection IEngineBuilder.NodeFeatures => NodeFeatures;

        internal IDictionary<string, object> Properties => EngineBuilder.Properties;
        IDictionary<string, object> IEngineBuilder.Properties => Properties;

        internal EngineBuilder EngineBuilder { get; }

        IServiceProvider IClusterNodeBuilder.ServiceProvider => Services;

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SHost"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NI2SHost"/>.</returns>
        public static NI2SHost Create(string[] args = null) =>
            new NI2SHostBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NI2SHostBuilder"/>.</returns>
        public static NI2SHostBuilder CreateBuilder() =>
            new(new());

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NI2SHostBuilder"/>.</returns>
        public static NI2SHostBuilder CreateBuilder(string[] args) =>
            new(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="NI2SNodeOptions"/> to configure the <see cref="NI2SHostBuilder"/>.</param>
        /// <returns>The <see cref="NI2SHostBuilder"/>.</returns>
        public static NI2SHostBuilder CreateBuilder(NI2SNodeOptions options) =>
            new(options);

        /// <summary>
        /// Start the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the startup of the <see cref="NI2SHost"/>.
        /// Successful completion indicates the HTTP server is ready to accept new requests.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _host.StartAsync(cancellationToken);

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the shutdown of the <see cref="NI2SHost"/>.
        /// Successful completion indicates that all the HTTP server has stopped.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _host.StopAsync(cancellationToken);

        /// <summary>
        /// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the entire runtime of the <see cref="NI2SHost"/> from startup to shutdown.
        /// </returns>
        public async Task RunAsync([StringSyntax(StringSyntaxAttribute.Uri)] string url = null)
        {
            await StartEngineAsync();
            await HostingAbstractionsHostExtensions.RunAsync(this);
        }

        /// <summary>
        /// Runs an application and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        public void Run([StringSyntax(StringSyntaxAttribute.Uri)] string url = null)
        {
            StartEngine();
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

        internal INiisEngine BuildNodeEngine() => EngineBuilder.Build();
        INiisEngine IEngineBuilder.Build() => BuildNodeEngine();

        // REVIEW: Should this be wrapping another type?
        IEngineBuilder IEngineBuilder.New()
        {
            var newBuilder = EngineBuilder.New();
            // Remove the route builder so branched pipelines have their own routing world
            newBuilder.Properties.Remove(GlobalNodeBuilderKey);
            return newBuilder;
        }

        /// <summary>
        /// Adds the middleware to the application request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns>An instance of <see cref="IEngineBuilder"/> after the operation has completed.</returns>
        public IEngineBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware)
        {
            EngineBuilder.Use(middleware);
            return this;
        }

        IEngineBuilder IClusterNodeBuilder.CreateEngineBuilder() => ((IEngineBuilder)this).New();

        private void StartEngine()
        {
            // Use a safe way to block async methods
            var engineStartTask = StartEngineAsync();
            engineStartTask.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task StartEngineAsync()
        {
            var engineContext = NI2SContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.IsDatabaseInstalled())
            {
                //TODO: log application start
                //await engine.Resolve<ILogger>().InformationAsync("Application started");

                //install and update plugins
                var pluginService = engineContext.Resolve<IPluginService>();
                await pluginService.InstallPluginsAsync();
                await pluginService.UpdatePluginsAsync();

                //update nopCommerce core and db
                var migrationManager = engineContext.Resolve<IMigrationManager>();
                var assembly = Assembly.GetAssembly(typeof(NI2SHostBuilderClusterExtensions));
                migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);
                assembly = Assembly.GetAssembly(typeof(IMigrationManager));
                migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update);

                ////insert new ACL permission if exists
                //var permissionService = engine.Resolve<IPermissionService>();
                //await permissionService.InsertPermissionsAsync();

                var taskScheduler = engineContext.Resolve<IClusterTaskScheduler>();
                await taskScheduler.InitializeAsync();
                await taskScheduler.StartSchedulerAsync();
            }
        }

        private string DebuggerToString()
        {
            return $@"ApplicationName = ""{Environment.ApplicationName}"", IsRunning = {(IsRunning ? "true" : "false")}";
        }

        // Web app is running if the app has been started and hasn't been stopped.
        private bool IsRunning => Lifetime.ApplicationStarted.IsCancellationRequested && !Lifetime.ApplicationStopped.IsCancellationRequested;

        internal sealed class NI2SHostDebugView(NI2SHost niisNode)
        {
            private readonly NI2SHost _niisNode = niisNode;

            public IServiceProvider Services => _niisNode.Services;
            public IConfiguration Configuration => _niisNode.Configuration;
            public INiisHostEnvironment Environment => _niisNode.Environment;
            public IHostApplicationLifetime Lifetime => _niisNode.Lifetime;
            public ILogger Logger => _niisNode.Logger;
            //public string Urls => string.Join(", ", _niisNode.Urls);
            //public IReadOnlyList<Endpoint> Endpoints
            //{
            //    get
            //    {
            //        var dataSource = _niisNode.Services.GetRequiredService<EndpointDataSource>();
            //        if (dataSource is CompositeEndpointDataSource compositeEndpointDataSource)
            //        {
            //            // The web app's data sources aren't registered until the routing middleware is. That often happens when the app is run.
            //            // We want endpoints to be available in the debug view before the app starts. Test if all the web app's the data sources are registered.
            //            if (compositeEndpointDataSource.DataSources.Intersect(_niisNode.DataSources).Count() == _niisNode.DataSources.Count)
            //            {
            //                // Data sources are centrally registered.
            //                return dataSource.Endpoints;
            //            }
            //            else
            //            {
            //                // Fallback to just the web app's data sources to support debugging before the web app starts.
            //                return new CompositeEndpointDataSource(_niisNode.DataSources).Endpoints;
            //            }
            //        }

            //        return dataSource.Endpoints;
            //    }
            //}
            public bool IsRunning => _niisNode.IsRunning;
            public IList<string> Middleware
            {
                get
                {
                    if (_niisNode.Properties.TryGetValue("__MiddlewareDescriptions", out var value) &&
                        value is IList<string> descriptions)
                    {
                        return descriptions;
                    }

                    throw new NotSupportedException("Unable to get configured middleware.");
                }
            }
        }

        internal static void ConfigureNI2SDefaults(INiisHostBuilder builder)
        {
            builder.ConfigureNI2SConfiguration((ctx, cb) =>
            {
                if (ctx.HostingEnvironment.IsDevelopment())
                {
                    LocalAssetsLoader.UseLocalAssets(ctx.HostingEnvironment, ctx.Configuration);
                }
            });

            ConfigureNI2SDefaultsWorker(
                builder.UseCluster(ConfigureKestrel),
                (services, config) =>
                {
                    //services.AddRouting()
                });

            //builder
            //    .UseIIS()
            //    .UseIISIntegration();
        }

        private static void ConfigureKestrel(NI2SHostBuilderContext builderContext, ClusterServerOptions options)
        {
            //options.Configure(builderContext.Configuration.GetSection("Kestrel"), reloadOnChange: true);
        }

        private static void ConfigureNI2SDefaultsWorker(INiisHostBuilder builder, Action<IServiceCollection, IConfiguration> configureCluster)
        {
            builder.ConfigureServices((hostingContext, services) =>
            {


                if (configureCluster == null)
                {
                    //services.AddRoutingCore()
                }
                else
                {
                    configureCluster(services, hostingContext.Configuration);
                }
            });
        }
    }







    //public sealed class NI2SHost : IHost, IEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    //{
    //    public NI2SHost(IHost host)
    //    {
    //    }

    //    public static NI2SHost Create(string[] args) => new NI2SHostBuilder(new() { Args = args }).Build();

    //    public static NI2SHostBuilder CreateBuilder(string[] args) => new(new() { Args = args });
    //    public static NI2SHostBuilder CreateBuilder(NI2SNodeOptions options) => new(options);


    //}
}
