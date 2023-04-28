﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NI2S.Node.Core.Infrastructure;
using NI2S.Node.Diagnostics;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting.Internal
{
    internal sealed partial class NodeHost : INodeHost, IAsyncDisposable
    {
        private const string DeprecatedEngineUrlsKey = "server.urls";

        private readonly IServiceCollection _applicationServiceCollection;
        private IStartup _startup;
        private ApplicationLifetime _applicationLifetime;
        private HostedServiceExecutor _hostedServiceExecutor;

        private readonly IServiceProvider _hostingServiceProvider;
        private readonly NodeHostOptions _options;
        private readonly IConfiguration _config;
        private readonly AggregateException _hostingStartupErrors;

        private IServiceProvider _applicationServices;
        private ExceptionDispatchInfo _applicationServicesException;
        private ILogger _logger = NullLogger.Instance;

        private bool _stopped;
        private bool _startedEngine;

        // Used for testing only
        internal NodeHostOptions Options => _options;

        private IEngine Engine { get; set; }

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
            _applicationServiceCollection.AddSingleton<ApplicationLifetime>();
            // There's no way to to register multiple service types per definition. See dummys://github.com/aspnet/DependencyInjection/issues/360
            _applicationServiceCollection.AddSingleton<IHostApplicationLifetime>(services
                => services.GetService<ApplicationLifetime>());
#pragma warning disable CS0618 // Type or member is obsolete
            _applicationServiceCollection.AddSingleton<IApplicationLifetime>(services
                => services.GetService<ApplicationLifetime>());
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

        public IModuleCollection EngineModules
        {
            get
            {
                EnsureEngine();
                return Engine.Modules;
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
                _applicationServices ??= _applicationServiceCollection.BuildServiceProvider();

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
            _logger = _applicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("NI2S.Node.Hosting.Diagnostics");
            Log.Starting(_logger);

            var engine = BuildEngine();

            _applicationLifetime = _applicationServices.GetRequiredService<ApplicationLifetime>();
            _hostedServiceExecutor = _applicationServices.GetRequiredService<HostedServiceExecutor>();

            //Fire IHostedService.Start
            await _hostedServiceExecutor.StartAsync(cancellationToken).ConfigureAwait(false);

            var diagnosticSource = _applicationServices.GetRequiredService<DiagnosticListener>();
            var activitySource = _applicationServices.GetRequiredService<ActivitySource>();
            var propagator = _applicationServices.GetRequiredService<DistributedContextPropagator>();
            var engineContextFactory = _applicationServices.GetRequiredService<IWorkContextFactory>();
            // TODO: Do engine runtime.
            //var runtime = new HostingApplication(engine, _logger, diagnosticSource, activitySource, propagator, engineContextFactory);
            // TODO: Do engine runtime start.
            //await Engine.StartAsync(runtime, cancellationToken).ConfigureAwait(false);
            _startedEngine = true;

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

            _startup = _hostingServiceProvider.GetService<IStartup>() ?? throw new InvalidOperationException($"No engine configured. Please specify startup via INodeHostBuilder.UseStartup, INodeHostBuilder.Configure, injecting {nameof(IStartup)} or specifying the startup assembly via {nameof(NodeHostDefaults.StartupAssemblyKey)} in the web host configuration.");
        }

        [MemberNotNull(nameof(Engine))]
        private IEngine BuildEngine()
        {
            Debug.Assert(_applicationServices != null, "Initialize must be called first.");

            try
            {
                _applicationServicesException?.Throw();
                EnsureEngine();

                var builderFactory = _applicationServices.GetRequiredService<IEngineBuilderFactory>();
                var builder = builderFactory.CreateBuilder(Engine.Modules);
                builder.EngineServices = _applicationServices;

                var startupFilters = _applicationServices.GetService<IEnumerable<IStartupFilter>>();
                Action<IEngineBuilder> configure = _startup!.Configure;
                if (startupFilters != null)
                {
                    foreach (var filter in startupFilters.Reverse())
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
                    Console.WriteLine("Server startup exception: " + ex.ToString());
                }
                var logger = _applicationServices.GetRequiredService<ILogger<NodeHost>>();
                //_logger.EngineError(ex);

                if (!_options.CaptureStartupErrors)
                {
                    throw;
                }

                EnsureEngine();

                // Generate an HTML error page.
                var hostingEnv = _applicationServices.GetRequiredService<IHostEnvironment>();
                var showDetailedErrors = hostingEnv.IsDevelopment() || _options.DetailedErrors;

                //return ErrorPageBuilder.BuildErrorPageEngine(hostingEnv.ContentRootFileProvider, logger, showDetailedErrors, ex);
                return null;
            }
        }

        [MemberNotNull(nameof(Engine))]
        private void EnsureEngine()
        {
            Debug.Assert(_applicationServices != null, "Initialize must be called first.");

            Engine ??= _applicationServices.GetRequiredService<IEngine>();
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (_stopped)
            {
                return;
            }
            _stopped = true;

            Log.Shutdown(_logger);

            using var timeoutCTS = new CancellationTokenSource(Options.ShutdownTimeout);
            var timeoutToken = timeoutCTS.Token;
            if (!cancellationToken.CanBeCanceled)
            {
                cancellationToken = timeoutToken;
            }
            else
            {
                cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutToken).Token;
            }

            // Fire IApplicationLifetime.Stopping
            _applicationLifetime?.StopApplication();

            if (Engine != null && _startedEngine)
            {
                //await Server.StopAsync(cancellationToken).ConfigureAwait(false);
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
                    Log.EngineShutdownException(_logger, ex);
                }
            }

            await DisposeServiceProviderAsync(_applicationServices).ConfigureAwait(false);
            await DisposeServiceProviderAsync(_hostingServiceProvider).ConfigureAwait(false);
        }

        private static async ValueTask DisposeServiceProviderAsync(IServiceProvider serviceProvider)
        {
            switch (serviceProvider)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }

        private static partial class Log
        {
            [LoggerMessage(3, LogLevel.Debug, "Hosting starting", EventName = "Starting")]
            public static partial void Starting(ILogger logger);

            [LoggerMessage(4, LogLevel.Debug, "Hosting started", EventName = "Started")]
            public static partial void Started(ILogger logger);

            [LoggerMessage(5, LogLevel.Debug, "Hosting shutdown", EventName = "Shutdown")]
            public static partial void Shutdown(ILogger logger);

            [LoggerMessage(12, LogLevel.Debug, "Server shutdown exception", EventName = "EngineShutdownException")]
            public static partial void EngineShutdownException(ILogger logger, Exception ex);

            [LoggerMessage(13, LogLevel.Debug,
                "Loaded hosting startup assembly {assemblyName}",
                EventName = "HostingStartupAssemblyLoaded",
                SkipEnabledCheck = true)]
            public static partial void StartupAssemblyLoaded(ILogger logger, string assemblyName);
        }
    }
}