using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NI2S.Node.Configuration;
using NI2S.Node.Dummy;
using NI2S.Node.Engine.Modules;
using NI2S.Node.Hosting.Builder;
using NI2S.Node.Hosting.Infrastructure;
using NI2S.Node.Hosting.Internal;
using NI2S.Node.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    internal sealed partial class GenericNodeHostService : IHostedService
    {
        public GenericNodeHostService(IOptions<GenericNodeHostServiceOptions> options,
                                     IDummyServer server,
                                     ILoggerFactory loggerFactory,
                                     DiagnosticListener diagnosticListener,
                                     ActivitySource activitySource,
                                     DistributedContextPropagator propagator,
                                     IDummyContextFactory httpContextFactory,
                                     INodeBuilderFactory applicationBuilderFactory,
                                     IEnumerable<IStartupFilter> startupFilters,
                                     IConfiguration configuration,
                                     INodeHostEnvironment hostingEnvironment)
        {
            Options = options.Value;
            Server = server;
            Logger = loggerFactory.CreateLogger("Microsoft.AspNetCore.Hosting.Diagnostics");
            LifetimeLogger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
            DiagnosticListener = diagnosticListener;
            ActivitySource = activitySource;
            Propagator = propagator;
            DummyContextFactory = httpContextFactory;
            ApplicationBuilderFactory = applicationBuilderFactory;
            StartupFilters = startupFilters;
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public GenericNodeHostServiceOptions Options { get; }
        public IDummyServer Server { get; }
        public ILogger Logger { get; }
        // Only for high level lifetime events
        public ILogger LifetimeLogger { get; }
        public DiagnosticListener DiagnosticListener { get; }
        public ActivitySource ActivitySource { get; }
        public DistributedContextPropagator Propagator { get; }
        public IDummyContextFactory DummyContextFactory { get; }
        public INodeBuilderFactory ApplicationBuilderFactory { get; }
        public IEnumerable<IStartupFilter> StartupFilters { get; }
        public IConfiguration Configuration { get; }
        public INodeHostEnvironment HostingEnvironment { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            HostingEventSource.Log.HostStart();

            var serverAddressesFeature = Server.Features.Get<IDummyAddressesFeature>();
            var addresses = serverAddressesFeature?.Addresses;
            if (addresses != null && !addresses.IsReadOnly && addresses.Count == 0)
            {
                // TODO: NODE PORTS
                // We support reading "urls" from app configuration
                //var urls = Configuration[NodeHostDefaults.ServerUrlsKey];

                // TODO: NODE PORTS
                // But fall back to host settings
                //if (string.IsNullOrEmpty(urls))
                //{
                //    urls = Options.NodeHostOptions.ServerUrls;
                //}

                // TODO: NODE PORTS
                //var httpPorts = /*Configuration[NodeHostDefaults.HttpPortsKey] ??*/ string.Empty;
                //var httpsPorts = /*Configuration[NodeHostDefaults.HttpsPortsKey] ??*/ string.Empty;
                //if (string.IsNullOrEmpty(urls))
                //{
                //    // HTTP_PORTS and HTTPS_PORTS, these are lower priority than Urls.
                //    static string ExpandPorts(string ports, string scheme)
                //    {
                //        return string.Join(';',
                //            ports.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                //            .Select(port => $"{scheme}://*:{port}"));
                //    }

                //    var httpUrls = ExpandPorts(httpPorts, Uri.UriSchemeHttp);
                //    var httpsUrls = ExpandPorts(httpsPorts, Uri.UriSchemeHttps);
                //    urls = $"{httpUrls};{httpsUrls}";
                //}
                //else if (!string.IsNullOrEmpty(httpPorts) || !string.IsNullOrEmpty(httpsPorts))
                //{
                //    Logger.PortsOverridenByUrls(httpPorts, httpsPorts, urls);
                //}

                // TODO: NODE PORTS
                //if (!string.IsNullOrEmpty(urls))
                //{
                //    // We support reading "preferHostingUrls" from app configuration
                //    var preferHostingUrlsConfig = Configuration[NodeHostDefaults.PreferHostingUrlsKey];

                //    // But fall back to host settings
                //    if (!string.IsNullOrEmpty(preferHostingUrlsConfig))
                //    {
                //        serverAddressesFeature!.PreferHostingUrls = NodeHostUtilities.ParseBool(preferHostingUrlsConfig);
                //    }
                //    else
                //    {
                //        serverAddressesFeature!.PreferHostingUrls = Options.NodeHostOptions.PreferHostingUrls;
                //    }

                //    foreach (var value in urls.Split(';', StringSplitOptions.RemoveEmptyEntries))
                //    {
                //        addresses.Add(value);
                //    }
                //}
            }

            MessageDelegate application = null;

            try
            {
                var configure = Options.ConfigureApplication ?? throw new InvalidOperationException($"No application configured. Please specify an application via INodeHostBuilder.UseStartup, INodeHostBuilder.Configure, or specifying the startup assembly via {nameof(NodeHostDefaults.StartupAssemblyKey)} in the web host configuration.");
                var builder = ApplicationBuilderFactory.CreateBuilder(Server.Features);

                foreach (var filter in StartupFilters.Reverse())
                {
                    configure = filter.Configure(configure);
                }

                configure(builder);

                // Build the request pipeline
                application = builder.Build();
            }
            catch (Exception ex)
            {
                Logger.ApplicationError(ex);

                if (!Options.NodeHostOptions.CaptureStartupErrors)
                {
                    throw;
                }

                var showDetailedErrors = HostingEnvironment.IsDevelopment() || Options.NodeHostOptions.DetailedErrors;

                //TODO: ERROR JANDLIN
                //application = ErrorPageBuilder.BuildErrorPageApplication(HostingEnvironment.ContentRootFileProvider, Logger, showDetailedErrors, ex);
            }

            var httpApplication = new DummyHostingApplication(application, Logger, DiagnosticListener, ActivitySource, Propagator, DummyContextFactory);

            await Server.StartAsync(httpApplication, cancellationToken);
            HostingEventSource.Log.ServerReady();

            if (addresses != null)
            {
                foreach (var address in addresses)
                {
                    Log.ListeningOnAddress(LifetimeLogger, address);
                }
            }

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
                await Server.StopAsync(cancellationToken);
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
