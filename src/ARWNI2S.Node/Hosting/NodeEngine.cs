﻿using ARWNI2S.Clustering.Builder;
using ARWNI2S.Configuration;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Features;
using ARWNI2S.Entities.Clustering;
using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Configuration.Options;
using ARWNI2S.Node.Hosting.Extensions;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Node.Hosting
{
    public sealed class NodeEngine : IHost, IEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    {
        internal const string GlobalClusterNodeBuilderKey = "__GlobalClusterNodeBuilder";

        private readonly IHost _host;
        //private readonly List<RelayDataSource> _dataSources = [];

        internal NodeEngine(IHost host)
        {
            _host = host;
            EngineBuilder = new EngineBuilder(host.Services, NodeFeatures);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName ?? nameof(NodeEngine));

            Properties[GlobalClusterNodeBuilderKey] = this;
        }

        /// <summary>
        /// The engine's configured services.
        /// </summary>
        public IServiceProvider Services => _host.Services;

        /// <summary>
        /// The engine's configured <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

        /// <summary>
        /// The engine's configured <see cref="IHostEnvironment"/>.
        /// </summary>
        public IHostEnvironment Environment => _host.Services.GetRequiredService<IHostEnvironment>();

        /// <summary>
        /// Allows consumers to be notified of engine lifetime events.
        /// </summary>
        public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

        /// <summary>
        /// The default logger for the engine.
        /// </summary>
        public ILogger Logger { get; }

        ///// <summary>
        ///// The list of URLs that the HTTP server is bound to.
        ///// </summary>
        //public ICollection<string> Urls => ServerFeatures.GetRequiredFeature<IServerAddressesFeature>().Addresses;

        IServiceProvider IEngineBuilder.EngineServices
        {
            get => EngineBuilder.EngineServices;
            set => EngineBuilder.EngineServices = value;
        }

        internal IFeatureCollection NodeFeatures => _host.Services.GetRequiredService<INiisNode>().Features;
        IFeatureCollection IEngineBuilder.NodeFeatures => NodeFeatures;

        internal IDictionary<string, object> Properties => EngineBuilder.Properties;
        IDictionary<string, object> IEngineBuilder.Properties => Properties;

        //internal ICollection<RelayDataSource> DataSources => _dataSources;
        //ICollection<RelayDataSource> IClusterNodeBuilder.DataSources => DataSources;

        internal EngineBuilder EngineBuilder { get; }

        IServiceProvider IClusterNodeBuilder.ServiceProvider => _host.Services;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngine"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NodeEngine"/>.</returns>
        public static NodeEngine Create(string[] args = null) =>
            CreateDefaultBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NodeEngineBuilder"/>.</returns>
        public static NodeEngineBuilder CreateBuilder() =>
            CreateDefaultBuilder(new());

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NodeEngineBuilder"/>.</returns>
        public static NodeEngineBuilder CreateBuilder(string[] args) =>
            CreateDefaultBuilder(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="NodeEngineOptions"/> to configure the <see cref="NodeEngineBuilder"/>.</param>
        /// <returns>The <see cref="NodeEngineBuilder"/>.</returns>
        public static NodeEngineBuilder CreateBuilder(NodeEngineOptions options) =>
            CreateDefaultBuilder(options);

        private static NodeEngineBuilder CreateDefaultBuilder(NodeEngineOptions options)
        {
            var builder = new NodeEngineBuilder(options);

            builder.Configuration.AddJsonFile(ConfigurationDefaults.SettingsFilePath, true, true);
            if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
            {
                var path = string.Format(ConfigurationDefaults.SettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
                builder.Configuration.AddJsonFile(path, true, true);
            }
            builder.Configuration.AddEnvironmentVariables();

            //load application settings
            builder.Services.ConfigureEngineSettings(builder);

            var appSettings = Singleton<NI2SSettings>.Instance;
            var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

            if (useAutofac)
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            else
                builder.Host.UseDefaultServiceProvider(options =>
                {
                    //we don't validate the scopes, since at the app start and the initial configuration we need 
                    //to resolve some services (registered as "scoped") through the root container
                    options.ValidateScopes = false;
                    options.ValidateOnBuild = true;
                });

            //add services to the application and configure service provider
            builder.Services.ConfigureEngineServices(builder);

            return builder;
        }

        /// <summary>
        /// Start the engine.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the startup of the <see cref="NodeEngine"/>.
        /// Successful completion indicates the HTTP server is ready to accept new requests.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _host.StartAsync(cancellationToken);

        /// <summary>
        /// Shuts down the engine.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the shutdown of the <see cref="NodeEngine"/>.
        /// Successful completion indicates that all the HTTP server has stopped.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _host.StopAsync(cancellationToken);

        /// <summary>
        /// Runs a engine and returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the entire runtime of the <see cref="NodeEngine"/> from startup to shutdown.
        /// </returns>
        public Task RunAsync([StringSyntax(StringSyntaxAttribute.Uri)] string url = null)
        {
            Listen(url);
            return HostingAbstractionsHostExtensions.RunAsync(this);
        }

        /// <summary>
        /// Runs a engine and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        public void Run([StringSyntax(StringSyntaxAttribute.Uri)] string url = null)
        {
            Listen(url);
            HostingAbstractionsHostExtensions.Run(this);
        }

        /// <summary>
        /// Disposes the engine.
        /// </summary>
        void IDisposable.Dispose() => _host.Dispose();

        /// <summary>
        /// Disposes the engine.
        /// </summary>
        public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

        internal UpdateDelegate BuildFrameDelegate() => EngineBuilder.Build();
        UpdateDelegate IEngineBuilder.Build() => BuildFrameDelegate();

        // REVIEW: Should this be wrapping another type?
        IEngineBuilder IEngineBuilder.New()
        {
            var newBuilder = EngineBuilder.New();
            // Remove the route builder so branched pipelines have their own routing world
            newBuilder.Properties.Remove(GlobalClusterNodeBuilderKey);
            return newBuilder;
        }

        /// <summary>
        /// Adds the frame processor to the engine request pipeline.
        /// </summary>
        /// <param name="frameProcessor">The frame processor.</param>
        /// <returns>An instance of <see cref="IEngineBuilder"/> after the operation has completed.</returns>
        public IEngineBuilder Use(Func<UpdateDelegate, UpdateDelegate> frameProcessor)
        {
            EngineBuilder.Use(frameProcessor);
            return this;
        }

        IEngineBuilder IClusterNodeBuilder.CreateEngineBuilder() => ((IEngineBuilder)this).New();

        private void Listen(string url)
        {
            if (url is null)
            {
                return;
            }

            //var addresses = ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
            //if (addresses is null)
            //{
            //    throw new InvalidOperationException($"Changing the URL is not supported because no valid {nameof(IServerAddressesFeature)} was found.");
            //}
            //if (addresses.IsReadOnly)
            //{
            //    throw new InvalidOperationException($"Changing the URL is not supported because {nameof(IServerAddressesFeature.Addresses)} {nameof(ICollection<string>.IsReadOnly)}.");
            //}

            //addresses.Clear();
            //addresses.Add(url);
        }

        private string DebuggerToString()
        {
            return $@"EngineName = ""{Environment.ApplicationName}"", IsRunning = {(IsRunning ? "true" : "false")}";
        }

        // Node engine is running if the engine has been started and hasn't been stopped.
        private bool IsRunning => Lifetime.ApplicationStarted.IsCancellationRequested && !Lifetime.ApplicationStopped.IsCancellationRequested;

    }
}
