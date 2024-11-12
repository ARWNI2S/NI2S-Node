using ARWNI2S.Infrastructure.Engine;
using ARWNI2S.Runtime.Hosting.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ARWNI2S.Runtime.Hosting
{
    internal sealed class HostingEngine : IEngine<HostingEngine.Context>
    {
        private readonly FrameDelegate _application;
        private readonly IFrameContextFactory _engineContextFactory;
        private readonly DefaultEngineContextFactory _defaultEngineContextFactory;
        private readonly HostingEngineDiagnostics _diagnostics;

        public HostingEngine(
            FrameDelegate application,
            ILogger logger,
            DiagnosticListener diagnosticSource,
            ActivitySource activitySource,
            DistributedContextPropagator propagator,
            IFrameContextFactory engineContextFactory,
            HostingEventSource eventSource,
            HostingMetrics metrics)
        {
            _application = application;
            _diagnostics = new HostingEngineDiagnostics(logger, diagnosticSource, activitySource, propagator, eventSource, metrics);
            if (engineContextFactory is DefaultEngineContextFactory factory)
            {
                _defaultEngineContextFactory = factory;
            }
            else
            {
                _engineContextFactory = engineContextFactory;
            }
        }

        // Set up the request
        public Context CreateContext(IFeatureCollection contextFeatures)
        {
            Context hostContext;
            if (contextFeatures is IHostContextContainer<Context> container)
            {
                hostContext = container.HostContext;
                if (hostContext is null)
                {
                    hostContext = new Context();
                    container.HostContext = hostContext;
                }
            }
            else
            {
                // Server doesn't support pooling, so create a new Context
                hostContext = new Context();
            }

            FrameContext engineContext;
            if (_defaultEngineContextFactory != null)
            {
                var defaultNI2SContext = (DefaultEngineContext)hostContext.EngineContext;
                if (defaultNI2SContext is null)
                {
                    engineContext = _defaultEngineContextFactory.Create(contextFeatures);
                    hostContext.EngineContext = engineContext;
                }
                else
                {
                    _defaultEngineContextFactory.Initialize(defaultNI2SContext, contextFeatures);
                    engineContext = defaultNI2SContext;
                }
            }
            else
            {
                engineContext = _engineContextFactory!.Create(contextFeatures);
                hostContext.EngineContext = engineContext;
            }

            _diagnostics.BeginRequest(engineContext, hostContext);
            return hostContext;
        }

        // Execute the request
        public Task ProcessFrameAsync(Context context)
        {
            return _application(context.EngineContext!);
        }

        // Clean up the request
        public void DisposeContext(Context context, Exception exception)
        {
            var engineContext = context.EngineContext!;
            _diagnostics.RequestEnd(engineContext, exception, context);

            if (_defaultEngineContextFactory != null)
            {
                _defaultEngineContextFactory.Dispose((DefaultEngineContext)engineContext);

                if (_defaultEngineContextFactory.EngineContextAccessor != null)
                {
                    // Clear the EngineContext if the accessor was used. It's likely that the lifetime extends
                    // past the end of the ni2s request and we want to avoid changing the reference from under
                    // consumers.
                    context.EngineContext = null;
                }
            }
            else
            {
                _engineContextFactory!.Dispose(engineContext);
            }

            _diagnostics.ContextDisposed(context);

            // Reset the context as it may be pooled
            context.Reset();
        }

        internal sealed class Context
        {
            public FrameContext EngineContext { get; set; }
            public IDisposable Scope { get; set; }
            public Activity Activity
            {
                get => EngineActivityFeature?.Activity;
                set
                {
                    if (EngineActivityFeature is null)
                    {
                        if (value != null)
                        {
                            EngineActivityFeature = new EngineActivityFeature(value);
                        }
                    }
                    else
                    {
                        EngineActivityFeature.Activity = value!;
                    }
                }
            }
            internal HostingRequestStartingLog? StartLog { get; set; }

            public long StartTimestamp { get; set; }
            internal bool HasDiagnosticListener { get; set; }
            public bool MetricsEnabled { get; set; }
            public bool EventLogEnabled { get; set; }

            internal EngineActivityFeature? EngineActivityFeature;
            internal EngineMetricsTagsFeature? MetricsTagsFeature;

            public void Reset()
            {
                // Not resetting EngineContext here as we pool it on the Context

                Scope = null;
                Activity = null;
                StartLog = null;

                StartTimestamp = 0;
                HasDiagnosticListener = false;
                MetricsEnabled = false;
                EventLogEnabled = false;
                MetricsTagsFeature?.Reset();
            }
        }
    }
}