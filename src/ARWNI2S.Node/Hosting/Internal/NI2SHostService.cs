// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Cluster;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Hosting;
using ARWNI2S.Node.Diagnostics;
using ARWNI2S.Node.Hosting.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using System.Diagnostics;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed partial class NI2SHostService : HostedLifecycleService
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
                                     DiagnosticListener diagnosticListener,
                                     ActivitySource activitySource,
                                     DistributedContextPropagator propagator,
                                     INiisContextFactory niisContextFactory,
                                     IEngineBuilderFactory engineBuilderFactory,
                                     IConfiguration configuration,
                                     INiisHostEnvironment hostingEnvironment,
                                     HostingMetrics hostingMetrics,
                                     ILoggerFactory loggerFactory,
                                     ILifecycleSubject lifecycle) : base(loggerFactory, lifecycle)
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

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            HostingEventSource.Log.HostStart();

            INiisEngine engine = null;

            try
            {
                var configure = Options.ConfigureEngine ?? throw new InvalidOperationException($"No application configured. Please specify an application via INiisHostBuilder.UseStartup, INiisHostBuilder.Configure.");

                var builder = EngineBuilderFactory.CreateBuilder(LocalNode.Modules);

                configure(builder);

                // Build the engine pipeline
                engine = builder.Build();
            }
            catch (Exception ex)
            {
                Logger.EngineError(ex);

                if (!Options.HostingOptions.CaptureStartupErrors)
                {
                    throw;
                }

                var showDetailedErrors = HostingEnvironment.IsDevelopment() || Options.HostingOptions.DetailedErrors;

                engine = ErrorMessageBuilder.BuildErrorMessageApplication(HostingEnvironment.ContentRootFileProvider, Logger, showDetailedErrors, ex);
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
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            //try
            //{
            //    await LocalNode.StopAsync(cancellationToken);
            //}
            //finally
            //{
            //    HostingEventSource.Log.HostStop();
            //}
            await base.StopAsync(cancellationToken);
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