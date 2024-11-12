using ARWNI2S.Infrastructure.Engine;
using ARWNI2S.Runtime.Configuration.Options;
using ARWNI2S.Runtime.Diagnostics;
using ARWNI2S.Runtime.Hosting.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ARWNI2S.Runtime.Hosting
{
    internal sealed partial class GenericNodeHostService : IHostedService
    {
        public GenericNodeHostService(IOptions<GenericNodeHostServiceOptions> options,
                                     IEngine engine,
                                     ILoggerFactory loggerFactory,
                                     DiagnosticListener diagnosticListener,
                                     ActivitySource activitySource,
                                     DistributedContextPropagator propagator,
                                     IEngineContextFactory engineContextFactory,
                                     IEngineBuilderFactory engineBuilderFactory,
                                     //IEnumerable<IStartupFilter> startupFilters,
                                     IConfiguration configuration,
                                     INodeHostEnvironment hostingEnvironment,
                                     HostingMetrics hostingMetrics)
        {
            Options = options.Value;
            Engine = engine;
            Logger = loggerFactory.CreateLogger("ARWNI2S.Runtime.Hosting.Diagnostics");
            LifecycleLogger = loggerFactory.CreateLogger("ARWNI2S.Infrastructure.Lifecycle");
            DiagnosticListener = diagnosticListener;
            ActivitySource = activitySource;
            Propagator = propagator;
            EngineContextFactory = engineContextFactory;
            EngineBuilderFactory = engineBuilderFactory;
            //StartupFilters = startupFilters;
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            HostingMetrics = hostingMetrics;
        }

        public GenericNodeHostServiceOptions Options { get; }
        public IEngine Engine { get; }
        public ILogger Logger { get; }
        // Only for high level lifetime events
        public ILogger LifecycleLogger { get; }
        public DiagnosticListener DiagnosticListener { get; }
        public ActivitySource ActivitySource { get; }
        public DistributedContextPropagator Propagator { get; }
        public IEngineContextFactory EngineContextFactory { get; }
        public IEngineBuilderFactory EngineBuilderFactory { get; }
        //public IEnumerable<IStartupFilter> StartupFilters { get; }
        public IConfiguration Configuration { get; }
        public INodeHostEnvironment HostingEnvironment { get; }
        public HostingMetrics HostingMetrics { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            HostingEventSource.Log.HostStart();

            //var serverAddressesFeature = Server.Features.Get<IServerAddressesFeature>();
            //var addresses = serverAddressesFeature?.Addresses;
            //if (addresses != null && !addresses.IsReadOnly && addresses.Count == 0)
            //{
            //    // We support reading "urls" from app configuration
            //    var urls = Configuration[NodeHostDefaults.ServerUrlsKey];

            //    // But fall back to host settings
            //    if (string.IsNullOrEmpty(urls))
            //    {
            //        urls = Options.NodeHostOptions.ServerUrls;
            //    }

            //    var httpPorts = Configuration[NodeHostDefaults.HttpPortsKey] ?? string.Empty;
            //    var httpsPorts = Configuration[NodeHostDefaults.HttpsPortsKey] ?? string.Empty;
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
            //        var preferHostingUrlsConfig = Configuration[NodeHostDefaults.PreferHostingUrlsKey];

            //        // But fall back to host settings
            //        if (!string.IsNullOrEmpty(preferHostingUrlsConfig))
            //        {
            //            serverAddressesFeature!.PreferHostingUrls = NodeHostUtilities.ParseBool(preferHostingUrlsConfig);
            //        }
            //        else
            //        {
            //            serverAddressesFeature!.PreferHostingUrls = Options.NodeHostOptions.PreferHostingUrls;
            //        }

            //        foreach (var value in urls.Split(';', StringSplitOptions.RemoveEmptyEntries))
            //        {
            //            addresses.Add(value);
            //        }
            //    }
            //}

            //RequestDelegate? engine = null;

            //try
            //{
            //    var configure = Options.ConfigureEngine;

            //    if (configure == null)
            //    {
            //        throw new InvalidOperationException($"No engine configured. Please specify an engine via INodeHostBuilder.UseStartup, INodeHostBuilder.Configure, or specifying the startup assembly via {nameof(NodeHostDefaults.StartupAssemblyKey)} in the web host configuration.");
            //    }

            //    var builder = EngineBuilderFactory.CreateBuilder(Server.Features);

            //    foreach (var filter in Enumerable.Reverse(StartupFilters))
            //    {
            //        configure = filter.Configure(configure);
            //    }

            //    configure(builder);

            //    // Build the request pipeline
            //    engine = builder.Build();
            //}
            //catch (Exception ex)
            //{
            //    Logger.EngineError(ex);

            //    if (!Options.NodeHostOptions.CaptureStartupErrors)
            //    {
            //        throw;
            //    }

            //    var showDetailedErrors = HostingEnvironment.IsDevelopment() || Options.NodeHostOptions.DetailedErrors;

            //    engine = ErrorPageBuilder.BuildErrorPageEngine(HostingEnvironment.ContentRootFileProvider, Logger, showDetailedErrors, ex);
            //}

            //var httpEngine = new HostingEngine(engine, Logger, DiagnosticListener, ActivitySource, Propagator, HttpContextFactory, HostingEventSource.Log, HostingMetrics);

            //await Server.StartAsync(httpEngine, cancellationToken);
            //HostingEventSource.Log.ServerReady();

            //if (addresses != null)
            //{
            //    foreach (var address in addresses)
            //    {
            //        Log.ListeningOnAddress(LifecycleLogger, address);
            //    }
            //}

            //if (Logger.IsEnabled(LogLevel.Debug))
            //{
            //    foreach (var assembly in Options.NodeHostOptions.GetFinalHostingStartupAssemblies())
            //    {
            //        Log.StartupAssemblyLoaded(Logger, assembly);
            //    }
            //}

            //if (Options.HostingStartupExceptions != null)
            //{
            //    foreach (var exception in Options.HostingStartupExceptions.InnerExceptions)
            //    {
            //        Logger.HostingStartupAssemblyError(exception);
            //    }
            //}
            await Task.Delay(100);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Engine.StopAsync(cancellationToken);
            }
            finally
            {
                HostingEventSource.Log.HostStop();
            }
        }

        private static partial class Log
        {
            [LoggerMessage(14,
                LogLevel.Information,
                "Now listening on: {address}",
                EventName = "ListeningOnAddress")]
            public static partial void ListeningOnAddress(ILogger logger, string address);

            [LoggerMessage(13,
                LogLevel.Debug,
                "Loaded hosting startup assembly {assemblyName}",
                EventName = "HostingStartupAssemblyLoaded",
                SkipEnabledCheck = true)]
            public static partial void StartupAssemblyLoaded(ILogger logger, string assemblyName);
        }
    }
}
