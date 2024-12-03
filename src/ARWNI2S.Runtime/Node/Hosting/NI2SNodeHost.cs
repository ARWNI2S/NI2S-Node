using ARWNI2S.Configuration;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Configuration;
using ARWNI2S.Node.Hosting.Extensions;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Node.Hosting
{
    public class NI2SNodeHost : IHost, IEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    {
        private IHost _host;

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

        internal IFeatureCollection NodeFeatures => _host.Services.GetRequiredService<IEngine>().Features;
        IFeatureCollection IEngineBuilder.NodeFeatures => NodeFeatures;

        internal IDictionary<string, object> Properties => EngineBuilder.Properties;
        IDictionary<string, object> IEngineBuilder.Properties => Properties;

        internal ICollection<RelayDataSource> DataSources => _dataSources;
        ICollection<RelayDataSource> IClusterNodeBuilder.DataSources => DataSources;

        internal EngineBuilder EngineBuilder { get; }

        IServiceProvider IClusterNodeBuilder.ServiceProvider => _host.Services;

        internal NI2SNodeHost(IHost host)
        {
            _host = host;
            EngineBuilder = new EngineBuilder(host.Services/*, ServerFeatures*/);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName ?? nameof(NodeEngineHost));

            Properties[GlobalNodeRelayerBuilderKey] = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeHost"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NI2SNodeHost"/>.</returns>
        public static NI2SNodeHost Create(string[] args = null) =>
            NewBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NI2SNodeHostBuilder"/>.</returns>
        public static NI2SNodeHostBuilder CreateBuilder() =>
            NewBuilder(new());

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NI2SNodeHostBuilder"/>.</returns>
        public static NI2SNodeHostBuilder CreateBuilder(string[] args) =>
            NewBuilder(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="NI2SNodeOptions"/> to configure the <see cref="NI2SNodeHostBuilder"/>.</param>
        /// <returns>The <see cref="NI2SNodeHostBuilder"/>.</returns>
        public static NI2SNodeHostBuilder CreateBuilder(NI2SNodeOptions options) =>
            NewBuilder(options);

        private static NI2SNodeHostBuilder NewBuilder(NI2SNodeOptions options)
        {
            var builder = new NI2SNodeHostBuilder(options);

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

        internal static void ConfigureNodeDefaults(INodeHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((ctx, cb) =>
            {
                if (ctx.HostingEnvironment.IsDevelopment())
                {
                    StaticNodeAssetsLoader.UseStaticNodeAssets(ctx.HostingEnvironment, ctx.Configuration);
                }
            });

            ConfigureNodeDefaultsWorker(
                builder.UseCoreEngine(ConfigureCoreEngine),
                services =>
                {
                    //services.AddRouting();
                });

            builder
                .UseNetwork()
                .UseNI2SIntegration();
        }

        private static void ConfigureCoreEngine(NodeHostBuilderContext builderContext, EngineOptions options)
        {
            options.Configure(builderContext.Configuration.GetSection("CoreEngine"), reloadOnChange: true);
        }

        private static void ConfigureNodeDefaultsWorker(INodeHostBuilder builder, Action<IServiceCollection> configureRouting)
        {
            builder.ConfigureServices((hostingContext, services) =>
            {
                // Fallback
                //services.PostConfigure<HostFilteringOptions>(options =>
                //{
                //    if (options.AllowedHosts == null || options.AllowedHosts.Count() == 0)
                //    {
                //        // "AllowedHosts": "localhost;127.0.0.1;[::1]"
                //        var hosts = hostingContext.Configuration["AllowedHosts"]?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                //        // Fall back to "*" to disable.
                //        options.AllowedHosts = (hosts?.Length > 0 ? hosts : new[] { "*" });
                //    }
                //});
                //// Change notification
                //services.AddSingleton<IOptionsChangeTokenSource<HostFilteringOptions>>(
                //        new ConfigurationChangeTokenSource<HostFilteringOptions>(hostingContext.Configuration));

                //services.AddTransient<IStartupFilter, HostFilteringStartupFilter>();
                //services.AddTransient<IStartupFilter, ForwardedHeadersStartupFilter>();
                //services.AddTransient<IConfigureOptions<ForwardedHeadersOptions>, ForwardedHeadersOptionsSetup>();

                // Provide a way for the default host builder to configure routing. This probably means calling AddRouting.
                // A lambda is used here because we don't want to reference AddRouting directly because of trimming.
                // This avoids the overhead of calling AddRoutingCore multiple times on app startup.
                if (configureRouting == null)
                {
                    //services.AddRoutingCore();
                }
                else
                {
                    configureRouting(services);
                }
            });
        }
    }
}
