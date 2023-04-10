﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NI2S.Node.Core.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    internal sealed partial class GenericNodeHostService : IHostedService
    {
        public GenericNodeHostService(IOptions<GenericNodeHostServiceOptions> options,
                                     IEngine engine,
                                     ILoggerFactory loggerFactory,
                                     DiagnosticListener diagnosticListener,
                                     ActivitySource activitySource,
                                     DistributedContextPropagator propagator,
                                     //IDummyContextFactory dummyContextFactory,
                                     //IEngineBuilderFactory applicationBuilderFactory,
                                     IEnumerable<IStartupFilter> startupFilters,
                                     IConfiguration configuration,
                                     INodeHostEnvironment hostingEnvironment)
        {
            Options = options.Value;
            Engine = engine;
            Logger = loggerFactory.CreateLogger("Microsoft.AspNetCore.Hosting.Diagnostics");
            LifetimeLogger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
            DiagnosticListener = diagnosticListener;
            ActivitySource = activitySource;
            Propagator = propagator;
            //DummyContextFactory = dummyContextFactory;
            //EngineBuilderFactory = applicationBuilderFactory;
            StartupFilters = startupFilters;
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public GenericNodeHostServiceOptions Options { get; }
        public IEngine Engine { get; }
        public ILogger Logger { get; }
        // Only for high level lifetime events
        public ILogger LifetimeLogger { get; }
        public DiagnosticListener DiagnosticListener { get; }
        public ActivitySource ActivitySource { get; }
        public DistributedContextPropagator Propagator { get; }
        //public IDummyContextFactory DummyContextFactory { get; }
        //public IEngineBuilderFactory EngineBuilderFactory { get; }
        public IEnumerable<IStartupFilter> StartupFilters { get; }
        public IConfiguration Configuration { get; }
        public INodeHostEnvironment HostingEnvironment { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //HostingEventSource.Log.HostStart();

            //var engineAddressesFeature = Engine.Features.Get<IEngineAddressesFeature>();
            //var addresses = engineAddressesFeature?.Addresses;
            //if (addresses != null && !addresses.IsReadOnly && addresses.Count == 0)
            //{
            //    // We support reading "urls" from app configuration
            //    var urls = Configuration[NodeHostDefaults.EngineUrlsKey];

            //    // But fall back to host settings
            //    if (string.IsNullOrEmpty(urls))
            //    {
            //        urls = Options.NodeHostOptions.EngineUrls;
            //    }

            //    if (!string.IsNullOrEmpty(urls))
            //    {
            //        // We support reading "preferHostingUrls" from app configuration
            //        var preferHostingUrlsConfig = Configuration[NodeHostDefaults.PreferHostingUrlsKey];

            //        // But fall back to host settings
            //        if (!string.IsNullOrEmpty(preferHostingUrlsConfig))
            //        {
            //            engineAddressesFeature!.PreferHostingUrls = NodeHostUtilities.ParseBool(preferHostingUrlsConfig);
            //        }
            //        else
            //        {
            //            engineAddressesFeature!.PreferHostingUrls = Options.NodeHostOptions.PreferHostingUrls;
            //        }

            //        foreach (var value in urls.Split(';', StringSplitOptions.RemoveEmptyEntries))
            //        {
            //            addresses.Add(value);
            //        }
            //    }
            //}

            //RequestDelegate application = null;

            //try
            //{
            //    var configure = Options.ConfigureEngine ?? throw new InvalidOperationException($"No application configured. Please specify an application via INodeHostBuilder.UseStartup, INodeHostBuilder.Configure, or specifying the startup assembly via {nameof(NodeHostDefaults.StartupAssemblyKey)} in the web host configuration.");
            //    var builder = EngineBuilderFactory.CreateBuilder(Engine.Features);

            //    foreach (var filter in StartupFilters.Reverse())
            //    {
            //        configure = filter.Configure(configure);
            //    }

            //    configure(builder);

            //    // Build the request pipeline
            //    application = builder.Build();
            //}
            //catch (Exception ex)
            //{
            //    Logger.EngineError(ex);

            //    if (!Options.NodeHostOptions.CaptureStartupErrors)
            //    {
            //        throw;
            //    }

            //    var showDetailedErrors = HostingEnvironment.IsDevelopment() || Options.NodeHostOptions.DetailedErrors;

            //    application = ErrorPageBuilder.BuildErrorPageEngine(HostingEnvironment.ContentRootFileProvider, Logger, showDetailedErrors, ex);
            //}

            //var dummyEngine = new HostingEngine(application, Logger, DiagnosticListener, ActivitySource, Propagator, DummyContextFactory);

            //await Engine.StartAsync(dummyEngine, cancellationToken);
            //HostingEventSource.Log.EngineReady();

            //if (addresses != null)
            //{
            //    foreach (var address in addresses)
            //    {
            //        Log.ListeningOnAddress(LifetimeLogger, address);
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
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //try
            //{
            //    await Engine.StopAsync(cancellationToken);
            //}
            //finally
            //{
            //    HostingEventSource.Log.HostStop();
            //}
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
