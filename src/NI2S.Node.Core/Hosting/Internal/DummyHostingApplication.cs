using Microsoft.Extensions.Logging;
using NI2S.Node.Dummy;
using NI2S.Node.Engine.Modules;
using NI2S.Node.Hosting.Server;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting.Internal
{
    internal sealed class DummyHostingApplication : IDummyApplication<DummyHostingApplication.Context>
    {
        private readonly MessageDelegate _application;
        private readonly IDummyContextFactory _dummyContextFactory;
        private readonly DefaultDummyContextFactory _defaultDummyContextFactory;
        private readonly DummyHostingApplicationDiagnostics _diagnostics;

        public DummyHostingApplication(
            MessageDelegate application,
            ILogger logger,
            DiagnosticListener diagnosticSource,
            ActivitySource activitySource,
            DistributedContextPropagator propagator,
            IDummyContextFactory dummyContextFactory)
        {
            _application = application;
            _diagnostics = new DummyHostingApplicationDiagnostics(logger, diagnosticSource, activitySource, propagator);
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
        public Context CreateContext(IDummyFeatureCollection contextFeatures)
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
                    // past the end of the http request and we want to avoid changing the reference from under
                    // consumers.
                    context.DummyContext = null;
                }
            }
            else
            {
                _dummyContextFactory!.Dispose(dummyContext);
            }

            DummyHostingApplicationDiagnostics.ContextDisposed(context);

            // Reset the context as it may be pooled
            context.Reset();
        }

        internal sealed class Context
        {
            public DummyContext DummyContext { get; set; }
            public IDisposable Scope { get; set; }
            public Activity Activity
            {
                get => HttpActivityFeature?.Activity;
                set
                {
                    if (HttpActivityFeature is null)
                    {
                        //HttpActivityFeature = new ActivityFeature(value!);
                    }
                    else
                    {
                        HttpActivityFeature.Activity = value!;
                    }
                }
            }
            internal DummyHostingRequestStartingLog StartLog { get; set; }

            public long StartTimestamp { get; set; }
            internal bool HasDiagnosticListener { get; set; }
            public bool EventLogEnabled { get; set; }

            internal IDummyActivityFeature HttpActivityFeature;

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
