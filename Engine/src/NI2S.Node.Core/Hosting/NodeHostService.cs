// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NI2S.Node.Diagnostics;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    internal sealed partial class NodeHostService : IHostedService
    {
        public NodeHostService(IOptions<GenericNodeHostServiceOptions> options,
                                     INodeEngine engine,
                                     ILoggerFactory loggerFactory,
                                     DiagnosticListener diagnosticListener,
                                     ActivitySource activitySource,
                                     DistributedContextPropagator propagator,
                                     IWorkContextFactory workContextFactory,
                                     IEngineBuilderFactory engineBuilderFactory,
                                     IEnumerable<IStartupFilter> startupFilters,
                                     IConfiguration configuration,
                                     INodeHostEnvironment hostingEnvironment)
        {
            Options = options.Value;
            Engine = engine;
            Logger = loggerFactory.CreateLogger("NI2S.Node.Hosting.Diagnostics");
            LifetimeLogger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
            DiagnosticListener = diagnosticListener;
            ActivitySource = activitySource;
            Propagator = propagator;
            EngineContextFactory = workContextFactory;
            EngineBuilderFactory = engineBuilderFactory;
            StartupFilters = startupFilters;
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public GenericNodeHostServiceOptions Options { get; }
        public INodeEngine Engine { get; }
        public ILogger Logger { get; }
        // Only for high level lifetime events
        public ILogger LifetimeLogger { get; }
        public DiagnosticListener DiagnosticListener { get; }
        public ActivitySource ActivitySource { get; }
        public DistributedContextPropagator Propagator { get; }
        public IWorkContextFactory EngineContextFactory { get; }
        public IEngineBuilderFactory EngineBuilderFactory { get; }
        public IEnumerable<IStartupFilter> StartupFilters { get; }
        public IConfiguration Configuration { get; }
        public INodeHostEnvironment HostingEnvironment { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            HostingEventSource.Log.HostStart();

            //var engineAddressesModule = Engine.Modules.Get<IEngineAddressesModule>();
            //var addresses = engineAddressesModule?.Addresses;
            //if (addresses != null && !addresses.IsReadOnly && addresses.Count == 0) 
            //{
            //    // We support reading "urls" from app configuration
            //    var urls = Configuration[NodeHostDefaults.ServerUrlsKey];

            //    // But fall back to host settings
            //    if (string.IsNullOrEmpty(urls))
            //    {
            //        urls = Options.NodeHostOptions.ServerUrls;
            //    }

            //    if (!string.IsNullOrEmpty(urls))
            //    {
            //        // We support reading "preferHostingUrls" from app configuration
            //        var preferHostingUrlsConfig = Configuration[NodeHostDefaults.PreferHostingUrlsKey];

            //        // But fall back to host settings
            //        if (!string.IsNullOrEmpty(preferHostingUrlsConfig))
            //        {
            //            engineAddressesModule.PreferHostingUrls = NodeHostUtilities.ParseBool(preferHostingUrlsConfig);
            //        }
            //        else
            //        {
            //            engineAddressesModule.PreferHostingUrls = Options.NodeHostOptions.PreferHostingUrls;
            //        }

            //        foreach (var value in urls.Split(';', StringSplitOptions.RemoveEmptyEntries))
            //        {
            //            addresses.Add(value);
            //        }
            //    }
            //}

            INodeEngine application = null;

            try
            {
                var configure = Options.ConfigureEngine ?? throw new InvalidOperationException($"No application configured. Please specify an application via INodeHostBuilder.UseStartup, INodeHostBuilder.Configure, or specifying the startup assembly via {nameof(NodeHostDefaults.StartupAssemblyKey)} in the web host configuration.");
                var builder = EngineBuilderFactory.CreateBuilder(Engine.Modules);

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
                //Logger.EngineError(ex);

                if (!Options.NodeHostOptions.CaptureStartupErrors)
                {
                    throw;
                }

                var showDetailedErrors = HostingEnvironment.IsDevelopment() || Options.NodeHostOptions.DetailedErrors;

                //application = ErrorPageBuilder.BuildErrorPageEngine(HostingEnvironment.ContentRootFileProvider, Logger, showDetailedErrors, ex);
            }

            //var dummyEngine = new HostingApplication(application, Logger, DiagnosticListener, ActivitySource, Propagator, EngineContextFactory);

            //await Engine.StartAsync(dummyEngine, cancellationToken);
            //HostingEventSource.Log.EngineReady();

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
                    //Logger.HostingStartupAssemblyError(exception);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                //await Server.StopAsync(cancellationToken);
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
