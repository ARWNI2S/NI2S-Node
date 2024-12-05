using ARWNI2S.Clustering.Configuration.Options;
using ARWNI2S.Clustering.Hosting.Extensions;
using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Hosting.Extensions;
using ARWNI2S.Node.Hosting.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ARWNI2S.Node.Hosting
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="INodeHost"/> and <see cref="INodeHostBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class NodeHost
    {
        ///// <summary>
        ///// Initializes and starts a new <see cref="INodeHost"/> with pre-configured defaults.
        ///// See <see cref="CreateDefaultBuilder()"/> for details.
        ///// </summary>
        ///// <param name="engine">A delegate that handles requests to the engine.</param>
        ///// <returns>A started <see cref="INodeHost"/> that hosts the engine.</returns>
        //public static INodeHost Start(UpdateDelegate engine) =>
        //    Start(url: null!, engine: engine);

        ///// <summary>
        ///// Initializes and starts a new <see cref="INodeHost"/> with pre-configured defaults.
        ///// See <see cref="CreateDefaultBuilder()"/> for details.
        ///// </summary>
        ///// <param name="url">The URL the hosted engine will listen on.</param>
        ///// <param name="engine">A delegate that handles requests to the engine.</param>
        ///// <returns>A started <see cref="INodeHost"/> that hosts the engine.</returns>
        //public static INodeHost Start([StringSyntax(StringSyntaxAttribute.Uri)] string url, UpdateDelegate engine)
        //{
        //    var startupAssemblyName = engine.GetMethodInfo().DeclaringType!.Assembly.GetName().Name;
        //    return StartWith(url: url, configureServices: null, engine: engineBuilder => engineBuilder.Run(engine), engineName: startupAssemblyName);
        //}

        ///// <summary>
        ///// Initializes and starts a new <see cref="INodeHost"/> with pre-configured defaults.
        ///// See <see cref="CreateDefaultBuilder()"/> for details.
        ///// </summary>
        ///// <param name="routeBuilder">A delegate that configures the router for handling requests to the engine.</param>
        ///// <returns>A started <see cref="INodeHost"/> that hosts the engine.</returns>
        //public static INodeHost Start(Action<IRouteBuilder> routeBuilder) =>
        //    Start(url: null!, routeBuilder: routeBuilder);

        ///// <summary>
        ///// Initializes and starts a new <see cref="INodeHost"/> with pre-configured defaults.
        ///// See <see cref="CreateDefaultBuilder()"/> for details.
        ///// </summary>
        ///// <param name="url">The URL the hosted engine will listen on.</param>
        ///// <param name="routeBuilder">A delegate that configures the router for handling requests to the engine.</param>
        ///// <returns>A started <see cref="INodeHost"/> that hosts the engine.</returns>
        //public static INodeHost Start([StringSyntax(StringSyntaxAttribute.Uri)] string url, Action<IRouteBuilder> routeBuilder)
        //{
        //    var startupAssemblyName = routeBuilder.GetMethodInfo().DeclaringType!.Assembly.GetName().Name;
        //    return StartWith(url, services => services.AddRouting(), engineBuilder => engineBuilder.UseRouter(routeBuilder), engineName: startupAssemblyName);
        //}

        ///// <summary>
        ///// Initializes and starts a new <see cref="INodeHost"/> with pre-configured defaults.
        ///// See <see cref="CreateDefaultBuilder()"/> for details.
        ///// </summary>
        ///// <param name="engine">The delegate that configures the <see cref="IEngineBuilder"/>.</param>
        ///// <returns>A started <see cref="INodeHost"/> that hosts the engine.</returns>
        //public static INodeHost StartWith(Action<IEngineBuilder> engine) =>
        //    StartWith(url: null!, engine: engine);

        ///// <summary>
        ///// Initializes and starts a new <see cref="INodeHost"/> with pre-configured defaults.
        ///// See <see cref="CreateDefaultBuilder()"/> for details.
        ///// </summary>
        ///// <param name="url">The URL the hosted engine will listen on.</param>
        ///// <param name="engine">The delegate that configures the <see cref="IEngineBuilder"/>.</param>
        ///// <returns>A started <see cref="INodeHost"/> that hosts the engine.</returns>
        //public static INodeHost StartWith([StringSyntax(StringSyntaxAttribute.Uri)] string url, Action<IEngineBuilder> engine) =>
        //    StartWith(url: url, configureServices: null, engine: engine, engineName: null);

        //private static INodeHost StartWith(string url, Action<IServiceCollection> configureServices, Action<IEngineBuilder> engine, string engineName)
        //{
        //    var builder = CreateDefaultBuilder();

        //    if (!string.IsNullOrEmpty(url))
        //    {
        //        builder.UseUrls(url);
        //    }

        //    if (configureServices != null)
        //    {
        //        builder.ConfigureServices(configureServices);
        //    }

        //    builder.Configure(engine);

        //    if (!string.IsNullOrEmpty(engineName))
        //    {
        //        builder.UseSetting(NodeHostDefaults.EngineKey, engineName);
        //    }

        //    var host = builder.Build();

        //    host.Start();

        //    return host;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are enginelied to the returned <see cref="NodeHostBuilder"/>:
        ///     use Clustering as the web server and configure it using the engine's configuration providers,
        ///     set the <see cref="IHostEnvironment.ContentRootPath"/> to the result of <see cref="Directory.GetCurrentDirectory()"/>,
        ///     load <see cref="IConfiguration"/> from 'enginesettings.json' and 'enginesettings.[<see cref="IHostEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     configure the <see cref="ILoggerFactory"/> to log to the console and debug output,
        ///     adds the HostFiltering middleware,
        ///     adds the ForwardedHeaders middleware if ASPNETCORE_FORWARDEDHEADERS_ENABLED=true,
        ///     and enable IIS integration.
        /// </remarks>
        /// <returns>The initialized <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder CreateDefaultBuilder() =>
            CreateDefaultBuilder(args: null!);

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are enginelied to the returned <see cref="NodeHostBuilder"/>:
        ///     use Clustering as the web server and configure it using the engine's configuration providers,
        ///     set the <see cref="IHostEnvironment.ContentRootPath"/> to the result of <see cref="Directory.GetCurrentDirectory()"/>,
        ///     load <see cref="IConfiguration"/> from 'enginesettings.json' and 'enginesettings.[<see cref="IHostEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     load <see cref="IConfiguration"/> from supplied command line args,
        ///     configure the <see cref="ILoggerFactory"/> to log to the console and debug output,
        ///     configure the <see cref="INodeHostEnvironment.NodeRootFileProvider"/> to map static web assets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     adds the HostFiltering middleware,
        ///     adds the ForwardedHeaders middleware if ASPNETCORE_FORWARDEDHEADERS_ENABLED=true,
        ///     and enable IIS integration.
        /// </remarks>
        /// <param name="args">The command line args.</param>
        /// <returns>The initialized <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder CreateDefaultBuilder(string[] args)
        {
            var builder = new NodeHostBuilder();

            if (string.IsNullOrEmpty(builder.GetSetting(NodeHostDefaults.ContentRootKey)))
            {
                builder.UseContentRoot(Directory.GetCurrentDirectory());
            }
            if (args != null)
            {
                builder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());
            }

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                config.AddJsonFile("enginesettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"enginesettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                if (env.IsDevelopment())
                {
                    if (!string.IsNullOrEmpty(env.ApplicationName))
                    {
                        var engineAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (engineAssembly != null)
                        {
                            config.AddUserSecrets(engineAssembly, optional: true);
                        }
                    }
                }

                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.Configure(options =>
                {
                    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                                        | ActivityTrackingOptions.TraceId
                                                        | ActivityTrackingOptions.ParentId;
                });
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddEventSourceLogger();
            }).
            UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
            });

            ConfigureNodeDefaults(builder);

            return builder;
        }

        internal static void ConfigureNodeDefaults(INodeHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((ctx, cb) =>
            {
                if (ctx.HostingEnvironment.IsDevelopment())
                {
                    NodeAssetsLoader.UseLocalAssets(ctx.HostingEnvironment, ctx.Configuration);
                }
            });

            ConfigureNodeDefaultsWorker(
                builder.UseClustering(ConfigureClustering),
                services =>
                {
                    services.AddNetworking();
                });

            builder
                .UseOrleans()
                .UseOrleansIntegration();
        }

        internal static void ConfigureNodeDefaultsSlim(INodeHostBuilder builder)
        {
            ConfigureNodeDefaultsWorker(builder.UseClusteringCore().ConfigureClustering(ConfigureClustering), configureNetwork: null);
        }

        private static void ConfigureClustering(NodeHostBuilderContext builderContext, ClusteringServerOptions options)
        {
            options.Configure(builderContext.Configuration.GetSection("Clustering"), reloadOnChange: true);
        }

        private static void ConfigureNodeDefaultsWorker(INodeHostBuilder builder, Action<IServiceCollection> configureNetwork)
        {
            builder.ConfigureServices((hostingContext, services) =>
            {
                //// Fallback
                //services.PostConfigure<HostFilteringOptions>(options =>
                //{
                //    if (options.AllowedHosts == null || options.AllowedHosts.Count == 0)
                //    {
                //        // "AllowedHosts": "localhost;127.0.0.1;[::1]"
                //        var hosts = hostingContext.Configuration["AllowedHosts"]?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                //        // Fall back to "*" to disable.
                //        options.AllowedHosts = hosts?.Length > 0 ? hosts : new[] { "*" };
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
                // This avoids the overhead of calling AddRoutingCore multiple times on engine startup.
                //if (configureNetwork == null)
                //{
                //    services.AddRoutingCore();
                //}
                //else
                //{
                //    configureNetwork(services);
                //}
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with pre-configured defaults using typed Startup.
        /// </summary>
        /// <remarks>
        ///   The following defaults are enginelied to the returned <see cref="NodeHostBuilder"/>:
        ///     use Clustering as the web server and configure it using the engine's configuration providers,
        ///     set the <see cref="IHostEnvironment.ContentRootPath"/> to the result of <see cref="Directory.GetCurrentDirectory()"/>,
        ///     load <see cref="IConfiguration"/> from 'enginesettings.json' and 'enginesettings.[<see cref="IHostEnvironment.EnvironmentName"/>].json',
        ///     load <see cref="IConfiguration"/> from User Secrets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly,
        ///     load <see cref="IConfiguration"/> from environment variables,
        ///     load <see cref="IConfiguration"/> from supplied command line args,
        ///     configure the <see cref="ILoggerFactory"/> to log to the console and debug output,
        ///     enable IIS integration.
        /// </remarks>
        /// <typeparam name ="TStartup">The type containing the startup methods for the engine.</typeparam>
        /// <param name="args">The command line args.</param>
        /// <returns>The initialized <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder CreateDefaultBuilder<[DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] TStartup>(string[] args) where TStartup : class =>
            CreateDefaultBuilder(args).UseStartup<TStartup>();
    }
}