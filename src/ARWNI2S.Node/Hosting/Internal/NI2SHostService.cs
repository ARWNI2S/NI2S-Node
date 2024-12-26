using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Cluster;
using ARWNI2S.Engine.Diagnostics;
using ARWNI2S.Hosting;
using ARWNI2S.Node.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed partial class NI2SHostService : IHostedService
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="ClusterClient"/> class.
        ///// </summary>
        ///// <param name="serviceProvider">The service provider.</param>
        ///// <param name="runtimeClient">The runtime client.</param>
        ///// <param name="loggerFactory">Logger factory used to create loggers</param>
        ///// <param name="clientMessagingOptions">Messaging parameters</param>
        //public ClusterClient(IServiceProvider serviceProvider, OutsideRuntimeClient runtimeClient, ILoggerFactory loggerFactory, IOptions<ClientMessagingOptions> clientMessagingOptions)
        //{
        //    ValidateSystemConfiguration(serviceProvider);

        //    _runtimeClient = runtimeClient;
        //    _logger = loggerFactory.CreateLogger<ClusterClient>();
        //    _clusterClientLifecycle = new ClusterClientLifecycle(_logger);

        //    // register all lifecycle participants
        //    IEnumerable<ILifecycleParticipant<IClusterClientLifecycle>> lifecycleParticipants = ServiceProvider.GetServices<ILifecycleParticipant<IClusterClientLifecycle>>();
        //    foreach (var participant in lifecycleParticipants)
        //    {
        //        participant?.Participate(_clusterClientLifecycle);
        //    }

        //    static void ValidateSystemConfiguration(IServiceProvider serviceProvider)
        //    {
        //        var validators = serviceProvider.GetServices<IConfigurationValidator>();
        //        foreach (var validator in validators)
        //        {
        //            validator.ValidateConfiguration();
        //        }
        //    }
        //}



        public NI2SHostService(IOptions<NI2SHostServiceOptions> options,
                                     IClusterNode localNode,
                                     ILoggerFactory loggerFactory,
                                     DiagnosticListener diagnosticListener,
                                     ActivitySource activitySource,
                                     DistributedContextPropagator propagator,
                                     INiisContextFactory niisContextFactory,
                                     IEngineBuilderFactory engineBuilderFactory,
                                     IConfiguration configuration,
                                     INiisHostEnvironment hostingEnvironment,
                                     HostingMetrics hostingMetrics
            )
        {
            Options = options.Value;
            LocalNode = localNode;
            Logger = loggerFactory.CreateLogger("ARWNI2S.Hosting.Diagnostics");
            LifetimeLogger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
            DiagnosticListener = diagnosticListener;
            ActivitySource = activitySource;
            Propagator = propagator;
            ContextFactory = niisContextFactory;
            EngineBuilderFactory = engineBuilderFactory;
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            HostingMetrics = hostingMetrics;
        }

        public NI2SHostServiceOptions Options { get; }
        public IClusterNode LocalNode { get; }
        public ILogger Logger { get; }
        // Only for high level lifetime events
        public ILogger LifetimeLogger { get; }
        public DiagnosticListener DiagnosticListener { get; }
        public ActivitySource ActivitySource { get; }
        public DistributedContextPropagator Propagator { get; }
        public INiisContextFactory ContextFactory { get; }
        public IEngineBuilderFactory EngineBuilderFactory { get; }

        public IConfiguration Configuration { get; }
        public INiisHostEnvironment HostingEnvironment { get; }
        public HostingMetrics HostingMetrics { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            HostingEventSource.Log.HostStart();

            //var serverAddressesFeature = LocalNode.Features.Get<ILocalNodeAddressesFeature>();
            //var addresses = serverAddressesFeature?.Addresses;
            //if (addresses != null && !addresses.IsReadOnly && addresses.Count == 0)
            //{
            //    // We support reading "urls" from app configuration
            //    var urls = Configuration[WebHostDefaults.LocalNodeUrlsKey];

            //    // But fall back to host settings
            //    if (string.IsNullOrEmpty(urls))
            //    {
            //        urls = Options.WebHostOptions.LocalNodeUrls;
            //    }

            //    var httpPorts = Configuration[WebHostDefaults.HttpPortsKey] ?? string.Empty;
            //    var httpsPorts = Configuration[WebHostDefaults.HttpsPortsKey] ?? string.Empty;
            //    if (string.IsNullOrEmpty(urls))
            //    {
            //        // HTTP_PORTS and HTTPS_PORTS, these are lower priority than Urls.
            //        static string ExpandPorts(string ports, string scheme)
            //        {
            //            return string.Join(';',
            //                ports.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            //                .Select(port => $"{scheme}://*:{port}"));
            //        }

            //        var httpUrls = ExpandPorts(httpPorts, Uri.UriSchemeHttp);
            //        var httpsUrls = ExpandPorts(httpsPorts, Uri.UriSchemeHttps);
            //        urls = $"{httpUrls};{httpsUrls}";
            //    }
            //    else if (!string.IsNullOrEmpty(httpPorts) || !string.IsNullOrEmpty(httpsPorts))
            //    {
            //        Logger.PortsOverridenByUrls(httpPorts, httpsPorts, urls);
            //    }

            //    if (!string.IsNullOrEmpty(urls))
            //    {
            //        // We support reading "preferHostingUrls" from app configuration
            //        var preferHostingUrlsConfig = Configuration[WebHostDefaults.PreferHostingUrlsKey];

            //        // But fall back to host settings
            //        if (!string.IsNullOrEmpty(preferHostingUrlsConfig))
            //        {
            //            serverAddressesFeature!.PreferHostingUrls = WebHostUtilities.ParseBool(preferHostingUrlsConfig);
            //        }
            //        else
            //        {
            //            serverAddressesFeature!.PreferHostingUrls = Options.WebHostOptions.PreferHostingUrls;
            //        }

            //        foreach (var value in urls.Split(';', StringSplitOptions.RemoveEmptyEntries))
            //        {
            //            addresses.Add(value);
            //        }
            //    }
            //}

            INiisEngine engine = null;

            try
            {
                var configure = Options.ConfigureEngine ?? throw new InvalidOperationException($"No application configured. Please specify an application via INiisHostBuilder.UseStartup, INiisHostBuilder.Configure.");

                var builder = EngineBuilderFactory.CreateBuilder(LocalNode.Modules);

                //foreach (var filter in Enumerable.Reverse(StartupFilters))
                //{
                //    configure = filter.Configure(configure);
                //}

                configure(builder);

                // Build the engine pipeline
                engine = builder.Build();
            }
            catch (Exception ex)
            {
                Logger.EngineError(ex);

                //if (!Options.HostingOptions.CaptureStartupErrors)
                //{
                //    throw;
                //}

                //var showDetailedErrors = HostingEnvironment.IsDevelopment() || Options.HostingOptions.DetailedErrors;

                //engine = ErrorPageBuilder.BuildErrorPageApplication(HostingEnvironment.ContentRootFileProvider, Logger, showDetailedErrors, ex);
            }

            var engineHost = new EngineHost(engine, Logger, DiagnosticListener, ActivitySource, Propagator, ContextFactory, HostingEventSource.Log, HostingMetrics);

            await LocalNode.StartAsync(engineHost, cancellationToken);
            HostingEventSource.Log.NodeReady();

            //if (addresses != null)
            //{
            //    foreach (var address in addresses)
            //    {
            //        Log.ListeningOnAddress(LifetimeLogger, address);
            //    }
            //}

            //if (Logger.IsEnabled(LogLevel.Debug))
            //{
            //    foreach (var assembly in Options.HostingOptions.GetFinalHostingStartupAssemblies())
            //    {
            //        Log.StartupAssemblyLoaded(Logger, assembly);
            //    }
            //}

            //if (Options.HostingStartupExceptions != null)
            //{
            //    foreach (var exception in Options.HostingStartupExceptions.InnerExceptions)
            //    {
            //        //Logger.HostingStartupAssemblyError(exception);
            //    }
            //}
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //try
            //{
            //    await LocalNode.StopAsync(cancellationToken);
            //}
            //finally
            //{
            //    HostingEventSource.Log.HostStop();
            //}
            await Task.CompletedTask;
        }

        private static partial class Log
        {
            [LoggerMessage(14, LogLevel.Information,
                "Now listening on: {address}",
                EventName = "ListeningOnAddress")]
            public static partial void ListeningOnAddress(ILogger logger, string address);

            [LoggerMessage(13, LogLevel.Debug,
                "Loaded hosting startup assembly {assemblyName}",
                EventName = "HostingStartupAssemblyLoaded",
                SkipEnabledCheck = true)]
            public static partial void StartupAssemblyLoaded(ILogger logger, string assemblyName);
        }
    }
}