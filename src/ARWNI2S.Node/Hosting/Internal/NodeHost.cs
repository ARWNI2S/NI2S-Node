using ARWNI2S.Diagnostics;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Features;
using ARWNI2S.Entities.Clustering;
using ARWNI2S.Node.Configuration.Options;
using ARWNI2S.Node.Hosting.Diagnostics;
using ARWNI2S.Node.Hosting.Extensions;
using ARWNI2S.Node.Hosting.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed partial class NodeHost : INodeHost, IAsyncDisposable
    {
        private const string DeprecatedServerUrlsKey = "server.urls";

        private readonly IServiceCollection _applicationServiceCollection;
        private INodeStartup _startup;
        private EngineLifetime _applicationLifetime;
        private HostedServiceExecutor _hostedServiceExecutor;

        private readonly IServiceProvider _hostingServiceProvider;
        private readonly NodeHostOptions _options;
        private readonly IConfiguration _config;
        private readonly AggregateException _hostingStartupErrors;

        private IServiceProvider _applicationServices;
        private ExceptionDispatchInfo _applicationServicesException;
        private ILogger _logger = NullLogger.Instance;

        private bool _stopped;
        private bool _startedServer;

        // Used for testing only
        internal NodeHostOptions Options => _options;

        private INiisNode NiisNode { get; set; }

        public NodeHost(
            IServiceCollection appServices,
            IServiceProvider hostingServiceProvider,
            NodeHostOptions options,
            IConfiguration config,
            AggregateException hostingStartupErrors)
        {
            ArgumentNullException.ThrowIfNull(appServices);
            ArgumentNullException.ThrowIfNull(hostingServiceProvider);
            ArgumentNullException.ThrowIfNull(config);

            _config = config;
            _hostingStartupErrors = hostingStartupErrors;
            _options = options;
            _applicationServiceCollection = appServices;
            _hostingServiceProvider = hostingServiceProvider;
            _applicationServiceCollection.AddSingleton<EngineLifetime>();
            // There's no way to to register multiple service types per definition. See https://github.com/aspnet/DependencyInjection/issues/360
            _applicationServiceCollection.AddSingleton<IHostApplicationLifetime>(services
                => services.GetService<EngineLifetime>()!);
#pragma warning disable CS0618 // Type or member is obsolete
            _applicationServiceCollection.AddSingleton<IEngineLifetime>(services
                => services.GetService<EngineLifetime>()!);
            _applicationServiceCollection.AddSingleton<IApplicationLifetime>(services
                => services.GetService<EngineLifetime>()!);
#pragma warning restore CS0618 // Type or member is obsolete
            _applicationServiceCollection.AddSingleton<HostedServiceExecutor>();
        }

        public IServiceProvider Services
        {
            get
            {
                Debug.Assert(_applicationServices != null, "Initialize must be called before accessing services.");
                return _applicationServices;
            }
        }

        public IFeatureCollection NodeFeatures
        {
            get
            {
                EnsureServer();
                return NiisNode.Features;
            }
        }

        // Called immediately after the constructor so the properties can rely on it.
        public void Initialize()
        {
            try
            {
                EnsureEngineServices();
            }
            catch (Exception ex)
            {
                // EnsureEngineServices may have failed due to a missing or throwing Startup class.
                if (_applicationServices == null)
                {
                    _applicationServices = _applicationServiceCollection.BuildServiceProvider();
                }

                if (!_options.CaptureStartupErrors)
                {
                    throw;
                }

                _applicationServicesException = ExceptionDispatchInfo.Capture(ex);
            }
        }

        public void Start()
        {
            StartAsync().GetAwaiter().GetResult();
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            Debug.Assert(_applicationServices != null, "Initialize must be called first.");

            HostingEventSource.Log.HostStart();
            _logger = _applicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Microsoft.AspNetCore.Hosting.Diagnostics");
            Log.Starting(_logger);

            var application = BuildEngine();

            _applicationLifetime = _applicationServices.GetRequiredService<EngineLifetime>();
            _hostedServiceExecutor = _applicationServices.GetRequiredService<HostedServiceExecutor>();

            // Fire IHostedService.Start
            await _hostedServiceExecutor.StartAsync(cancellationToken).ConfigureAwait(false);

            var diagnosticSource = _applicationServices.GetRequiredService<DiagnosticListener>();
            var activitySource = _applicationServices.GetRequiredService<ActivitySource>();
            var propagator = _applicationServices.GetRequiredService<DistributedContextPropagator>();
            var niisContextFactory = _applicationServices.GetRequiredService<INiisContextFactory>();
            var hostingMetrics = _applicationServices.GetRequiredService<HostingMetrics>();
            var hostingApp = new HostingApplication(application, _logger, diagnosticSource, activitySource, propagator, niisContextFactory, HostingEventSource.Log, hostingMetrics);
            await NiisNode.StartAsync(hostingApp, cancellationToken).ConfigureAwait(false);
            _startedServer = true;

            // Fire IApplicationLifetime.Started
            _applicationLifetime?.NotifyStarted();

            Log.Started(_logger);

            // Log the fact that we did load hosting startup assemblies.
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                foreach (var assembly in _options.GetFinalHostingStartupAssemblies())
                {
                    Log.StartupAssemblyLoaded(_logger, assembly);
                }
            }

            if (_hostingStartupErrors != null)
            {
                foreach (var exception in _hostingStartupErrors.InnerExceptions)
                {
                    _logger.HostingStartupAssemblyError(exception);
                }
            }
        }

        private void EnsureEngineServices()
        {
            if (_applicationServices == null)
            {
                EnsureStartup();
                _applicationServices = _startup.ConfigureServices(_applicationServiceCollection);
            }
        }

        [MemberNotNull(nameof(_startup))]
        private void EnsureStartup()
        {
            if (_startup != null)
            {
                return;
            }

            var startup = _hostingServiceProvider.GetService<INodeStartup>();

            if (startup == null)
            {
                throw new InvalidOperationException($"No application configured. Please specify startup via INodeHostBuilder.UseStartup, INodeHostBuilder.Configure, injecting {nameof(INodeStartup)} or specifying the startup assembly via {nameof(NodeHostDefaults.StartupAssemblyKey)} in the web host configuration.");
            }

            _startup = startup;
        }

        [MemberNotNull(nameof(NiisNode))]
        private UpdateDelegate BuildEngine()
        {
            Debug.Assert(_applicationServices != null, "Initialize must be called first.");

            try
            {
                _applicationServicesException?.Throw();
                EnsureServer();

                var builderFactory = _applicationServices.GetRequiredService<IEngineBuilderFactory>();
                var builder = builderFactory.CreateBuilder(NiisNode.Features);
                builder.EngineServices = _applicationServices;

                var startupFilters = _applicationServices.GetService<IEnumerable<IStartupFilter>>();
                Action<IEngineBuilder> configure = _startup!.Configure;
                if (startupFilters != null)
                {
                    foreach (var filter in Enumerable.Reverse(startupFilters))
                    {
                        configure = filter.Configure(configure);
                    }
                }

                configure(builder);

                return builder.Build();
            }
            catch (Exception ex)
            {
                if (!_options.SuppressStatusMessages)
                {
                    // Write errors to standard out so they can be retrieved when not in development mode.
                    Console.WriteLine("Engine startup exception: " + ex.ToString());
                }
                var logger = _applicationServices.GetRequiredService<ILogger<NodeHost>>();
                _logger.EngineError(ex);

                if (!_options.CaptureStartupErrors)
                {
                    throw;
                }

                EnsureServer();

                // Generate an HTML error page.
                var hostingEnv = _applicationServices.GetRequiredService<IHostEnvironment>();
                var showDetailedErrors = hostingEnv.IsDevelopment() || _options.DetailedErrors;

                return ErrorDialogBuilder.BuildErrorPageEngine(hostingEnv.ContentRootFileProvider, logger, showDetailedErrors, ex);
            }
        }

        [MemberNotNull(nameof(NiisNode))]
        private void EnsureServer()
        {
            Debug.Assert(_applicationServices != null, "Initialize must be called first.");

            if (NiisNode == null)
            {
                NiisNode = _applicationServices.GetRequiredService<INiisNode>();
                //HACK
                //var serverAddressesFeature = NiisNode.Features?.Get<IServerAddressesFeature>();
                //var addresses = serverAddressesFeature?.Addresses;
                //if (addresses != null && !addresses.IsReadOnly && addresses.Count == 0)
                //{
                //    var urls = _config[NodeHostDefaults.ServerUrlsKey] ?? _config[DeprecatedServerUrlsKey];
                //    if (!string.IsNullOrEmpty(urls))
                //    {
                //        serverAddressesFeature!.PreferHostingUrls = _config[NodeHostDefaults.PreferHostingUrlsKey].ParseBool();

                //        foreach (var value in urls.Split(';', StringSplitOptions.RemoveEmptyEntries))
                //        {
                //            addresses.Add(value);
                //        }
                //    }
                //}
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (_stopped)
            {
                return;
            }
            _stopped = true;

            Log.Shutdown(_logger);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(Options.ShutdownTimeout);
            cancellationToken = cts.Token;

            // Fire IEngineLifetime.Stopping
            _applicationLifetime?.StopApplication();

            if (NiisNode != null && _startedServer)
            {
                await NiisNode.StopAsync(cancellationToken).ConfigureAwait(false);
            }

            // Fire the IHostedService.Stop
            if (_hostedServiceExecutor != null)
            {
                await _hostedServiceExecutor.StopAsync(cancellationToken).ConfigureAwait(false);
            }

            // Fire IApplicationLifetime.Stopped
            _applicationLifetime?.NotifyStopped();

            HostingEventSource.Log.HostStop();
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        public async ValueTask DisposeAsync()
        {
            if (!_stopped)
            {
                try
                {
                    await StopAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log.ServerShutdownException(_logger, ex);
                }
            }

            await DisposeServiceProviderAsync(_applicationServices).ConfigureAwait(false);
            await DisposeServiceProviderAsync(_hostingServiceProvider).ConfigureAwait(false);
        }

        private static ValueTask DisposeServiceProviderAsync(IServiceProvider serviceProvider)
        {
            switch (serviceProvider)
            {
                case IAsyncDisposable asyncDisposable:
                    return asyncDisposable.DisposeAsync();
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
            return default;
        }

        private static partial class Log
        {
            [LoggerMessage(3, LogLevel.Debug, "Hosting starting", EventName = "Starting")]
            public static partial void Starting(ILogger logger);

            [LoggerMessage(4, LogLevel.Debug, "Hosting started", EventName = "Started")]
            public static partial void Started(ILogger logger);

            [LoggerMessage(5, LogLevel.Debug, "Hosting shutdown", EventName = "Shutdown")]
            public static partial void Shutdown(ILogger logger);

            [LoggerMessage(12, LogLevel.Debug, "Server shutdown exception", EventName = "ServerShutdownException")]
            public static partial void ServerShutdownException(ILogger logger, Exception ex);

            [LoggerMessage(13, LogLevel.Debug,
                "Loaded hosting startup assembly {assemblyName}",
                EventName = "HostingStartupAssemblyLoaded",
                SkipEnabledCheck = true)]
            public static partial void StartupAssemblyLoaded(ILogger logger, string assemblyName);
        }
    }
}
