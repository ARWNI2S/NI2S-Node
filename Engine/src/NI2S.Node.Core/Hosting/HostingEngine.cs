using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using NI2S.Node.Hosting.Server;
using NI2S.Node.Hosting.Server.Abstractions;
using NI2S.Node.Dummy;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    internal sealed class HostingEngine : IDummyApplication<HostingEngine.Context>
    {
        private readonly RequestDelegate _application;
        private readonly IDummyContextFactory _dummyContextFactory;
        private readonly DefaultDummyContextFactory _defaultDummyContextFactory;
        private readonly HostingEngineDiagnostics _diagnostics;

        public HostingEngine(
            RequestDelegate application,
            ILogger logger,
            DiagnosticListener diagnosticSource,
            ActivitySource activitySource,
            DistributedContextPropagator propagator,
            IDummyContextFactory dummyContextFactory)
        {
            _application = application;
            _diagnostics = new HostingEngineDiagnostics(logger, diagnosticSource, activitySource, propagator);
            if (dummyContextFactory is DefaultDummyContextFactory factory)
            {
                _defaultDummyContextFactory = factory;
            }
            else
            {
                _dummyContextFactory = dummyContextFactory;
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

            DummyContext dummyContext;
            if (_defaultDummyContextFactory != null)
            {
                var defaultDummyContext = (DefaultDummyContext)hostContext.DummyContext;
                if (defaultDummyContext is null)
                {
                    dummyContext = _defaultDummyContextFactory.Create(contextFeatures);
                    hostContext.DummyContext = dummyContext;
                }
                else
                {
                    _defaultDummyContextFactory.Initialize(defaultDummyContext, contextFeatures);
                    dummyContext = defaultDummyContext;
                }
            }
            else
            {
                dummyContext = _dummyContextFactory!.Create(contextFeatures);
                hostContext.DummyContext = dummyContext;
            }

            _diagnostics.BeginRequest(dummyContext, hostContext);
            return hostContext;
        }

        // Execute the request
        public Task ProcessRequestAsync(Context context)
        {
            return _application(context.DummyContext!);
        }

        // Clean up the request
        public void DisposeContext(Context context, Exception exception)
        {
            var dummyContext = context.DummyContext!;
            _diagnostics.RequestEnd(dummyContext, exception, context);

            if (_defaultDummyContextFactory != null)
            {
                _defaultDummyContextFactory.Dispose((DefaultDummyContext)dummyContext);

                if (_defaultDummyContextFactory.DummyContextAccessor != null)
                {
                    // Clear the DummyContext if the accessor was used. It's likely that the lifetime extends
                    // past the end of the dummy request and we want to avoid changing the reference from under
                    // consumers.
                    context.DummyContext = null;
                }
            }
            else
            {
                _dummyContextFactory!.Dispose(dummyContext);
            }

            HostingEngineDiagnostics.ContextDisposed(context);

            // Reset the context as it may be pooled
            context.Reset();
        }

        internal sealed class Context
        {
            public DummyContext DummyContext { get; set; }
            public IDisposable Scope { get; set; }
            public Activity Activity
            {
                get => DummyActivityFeature?.Activity;
                set
                {
                    if (DummyActivityFeature is null)
                    {
                        DummyActivityFeature = new ActivityFeature(value!);
                    }
                    else
                    {
                        DummyActivityFeature.Activity = value!;
                    }
                }
            }
            internal HostingRequestStartingLog StartLog { get; set; }

            public long StartTimestamp { get; set; }
            internal bool HasDiagnosticListener { get; set; }
            public bool EventLogEnabled { get; set; }

            internal IDummyActivityFeature DummyActivityFeature;

            public void Reset()
            {
                // Not resetting DummyContext here as we pool it on the Context

                Scope = null;
                Activity = null;
                StartLog = null;

                StartTimestamp = 0;
                HasDiagnosticListener = false;
                EventLogEnabled = false;
            }
        }
    }
}