using ARWNI2S.Diagnostics;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Entities.Clustering;
using ARWNI2S.Node.Hosting.Diagnostics;
using ARWNI2S.Node.Hosting.Extensions;
using ARWNI2S.Node.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ARWNI2S.Node.Hosting.Generic
{
    internal sealed partial class GenericNodeHostService : IHostedService
    {
        public GenericNodeHostService(IOptions<GenericNodeHostServiceOptions> options,
                                     INiisNode niisNode,
                                     ILoggerFactory loggerFactory,
                                     DiagnosticListener diagnosticListener,
                                     ActivitySource activitySource,
                                     DistributedContextPropagator propagator,
                                     INiisContextFactory niisContextFactory,
                                     IEngineBuilderFactory engineBuilderFactory,
                                     IEnumerable<IStartupFilter> startupFilters,
                                     IConfiguration configuration,
                                     INodeHostEnvironment hostingEnvironment,
                                     HostingMetrics hostingMetrics)
        {
            Options = options.Value;
            NiisNode = niisNode;
            Logger = loggerFactory.CreateLogger("Microsoft.AspNetCore.Hosting.Diagnostics");
            LifetimeLogger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
            DiagnosticListener = diagnosticListener;
            ActivitySource = activitySource;
            Propagator = propagator;
            NiisContextFactory = niisContextFactory;
            EngineBuilderFactory = engineBuilderFactory;
            StartupFilters = startupFilters;
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            HostingMetrics = hostingMetrics;
        }

        public GenericNodeHostServiceOptions Options { get; }
        public INiisNode NiisNode { get; }
        public ILogger Logger { get; }
        // Only for high level lifetime events
        public ILogger LifetimeLogger { get; }
        public DiagnosticListener DiagnosticListener { get; }
        public ActivitySource ActivitySource { get; }
        public DistributedContextPropagator Propagator { get; }
        public INiisContextFactory NiisContextFactory { get; }
        public IEngineBuilderFactory EngineBuilderFactory { get; }
        public IEnumerable<IStartupFilter> StartupFilters { get; }
        public IConfiguration Configuration { get; }
        public INodeHostEnvironment HostingEnvironment { get; }
        public HostingMetrics HostingMetrics { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            HostingEventSource.Log.HostStart();

            //HACK
            //var serverAddressesFeature = NiisNode.Features.Get<INiisNodeAddressesFeature>();
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
            //            serverAddressesFeature!.PreferHostingUrls = preferHostingUrlsConfig.ParseBool();
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

            UpdateDelegate engine = null;

            try
            {
                var configure = Options.ConfigureEngine;

                if (configure == null)
                {
                    throw new InvalidOperationException($"No engine configured. Please specify an engine via INodeHostBuilder.UseStartup, INodeHostBuilder.Configure, or specifying the startup assembly via {nameof(NodeHostDefaults.StartupAssemblyKey)} in the web host configuration.");
                }

                var builder = EngineBuilderFactory.CreateBuilder(NiisNode.Features);

                foreach (var filter in Enumerable.Reverse(StartupFilters))
                {
                    configure = filter.Configure(configure);
                }

                configure(builder);

                // Build the request pipeline
                engine = builder.Build();
            }
            catch (Exception ex)
            {
                Logger.EngineError(ex);

                if (!Options.NodeHostOptions.CaptureStartupErrors)
                {
                    throw;
                }

                var showDetailedErrors = HostingEnvironment.IsDevelopment() || Options.NodeHostOptions.DetailedErrors;

                engine = ErrorDialogBuilder.BuildErrorPageEngine(HostingEnvironment.ContentRootFileProvider, Logger, showDetailedErrors, ex);
            }

            var httpEngine = new HostingApplication(engine, Logger, DiagnosticListener, ActivitySource, Propagator, NiisContextFactory, HostingEventSource.Log, HostingMetrics);

            await NiisNode.StartAsync(httpEngine, cancellationToken);
            HostingEventSource.Log.ServerReady();

            //HACK
            //if (addresses != null)
            //{
            //    foreach (var address in addresses)
            //    {
            //        Log.ListeningOnAddress(LifetimeLogger, address);
            //    }
            //}

            if (Logger.IsEnabled(LogLevel.Debug))
            {
                foreach (var assembly in Options.NodeHostOptions.GetFinalHostingStartupAssemblies())
                {
                    Log.StartupAssemblyLoaded(Logger, assembly);
                }
            }

            if (Options.HostingStartupExceptions != null)
            {
                foreach (var exception in Options.HostingStartupExceptions.InnerExceptions)
                {
                    Logger.HostingStartupAssemblyError(exception);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await NiisNode.StopAsync(cancellationToken);
            }
            finally
            {
                HostingEventSource.Log.HostStop();
            }
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