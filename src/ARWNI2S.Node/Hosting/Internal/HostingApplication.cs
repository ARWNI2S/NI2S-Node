using ARWNI2S.Engine;
using ARWNI2S.Infrastructure;
using ARWNI2S.Node.Hosting.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed class HostingApplication : INiisEngine<HostingApplication.Context>
    {
        //private readonly UpdateDelegate _application;
        //private readonly INiisContextFactory _niisContextFactory;
        //private readonly DefaultNiisContextFactory _defaultNiisContextFactory;
        //private readonly HostingApplicationDiagnostics _diagnostics;

        public HostingApplication(
            UpdateDelegate application,
            ILogger logger,
            DiagnosticListener diagnosticSource,
            ActivitySource activitySource,
            DistributedContextPropagator propagator,
            INiisContextFactory niisContextFactory,
            HostingEventSource eventSource,
            HostingMetrics metrics)
        {
            //_application = application;
            //_diagnostics = new HostingApplicationDiagnostics(logger, diagnosticSource, activitySource, propagator, eventSource, metrics);
            //if (niisContextFactory is DefaultNiisContextFactory factory)
            //{
            //    _defaultNiisContextFactory = factory;
            //}
            //else
            //{
            //    _niisContextFactory = niisContextFactory;
            //}
        }

        //// Set up the request
        //public Context CreateContext(IFeatureCollection contextFeatures)
        //{
        //    Context? hostContext;
        //    if (contextFeatures is IHostContextContainer<Context> container)
        //    {
        //        hostContext = container.HostContext;
        //        if (hostContext is null)
        //        {
        //            hostContext = new Context();
        //            container.HostContext = hostContext;
        //        }
        //    }
        //    else
        //    {
        //        // Server doesn't support pooling, so create a new Context
        //        hostContext = new Context();
        //    }

        //    NiisContext niisContext;
        //    if (_defaultNiisContextFactory != null)
        //    {
        //        var defaultNiisContext = (DefaultNiisContext?)hostContext.NiisContext;
        //        if (defaultNiisContext is null)
        //        {
        //            niisContext = _defaultNiisContextFactory.Create(contextFeatures);
        //            hostContext.NiisContext = niisContext;
        //        }
        //        else
        //        {
        //            _defaultNiisContextFactory.Initialize(defaultNiisContext, contextFeatures);
        //            niisContext = defaultNiisContext;
        //        }
        //    }
        //    else
        //    {
        //        niisContext = _niisContextFactory!.Create(contextFeatures);
        //        hostContext.NiisContext = niisContext;
        //    }

        //    _diagnostics.BeginRequest(niisContext, hostContext);
        //    return hostContext;
        //}

        //// Execute the request
        //public Task ProcessRequestAsync(Context context)
        //{
        //    return _application(context.NiisContext!);
        //}

        //// Clean up the request
        //public void DisposeContext(Context context, Exception? exception)
        //{
        //    var niisContext = context.NiisContext!;
        //    _diagnostics.RequestEnd(niisContext, exception, context);

        //    if (_defaultNiisContextFactory != null)
        //    {
        //        _defaultNiisContextFactory.Dispose((DefaultNiisContext)niisContext);

        //        if (_defaultNiisContextFactory.NiisContextAccessor != null)
        //        {
        //            // Clear the NiisContext if the accessor was used. It's likely that the lifetime extends
        //            // past the end of the http request and we want to avoid changing the reference from under
        //            // consumers.
        //            context.NiisContext = null;
        //        }
        //    }
        //    else
        //    {
        //        _niisContextFactory!.Dispose(niisContext);
        //    }

        //    _diagnostics.ContextDisposed(context);

        //    // Reset the context as it may be pooled
        //    context.Reset();
        //}

        internal sealed class Context
        {
            public NiisContext NiisContext { get; set; }
            public IDisposable Scope { get; set; }
            //public Activity Activity
            //{
            //    get => HttpActivityFeature?.Activity;
            //    set
            //    {
            //        if (HttpActivityFeature is null)
            //        {
            //            if (value != null)
            //            {
            //                HttpActivityFeature = new HttpActivityFeature(value);
            //            }
            //        }
            //        else
            //        {
            //            HttpActivityFeature.Activity = value!;
            //        }
            //    }
            //}
            //internal HostingRequestStartingLog StartLog { get; set; }

            public long StartTimestamp { get; set; }
            internal bool HasDiagnosticListener { get; set; }
            public bool MetricsEnabled { get; set; }
            public bool EventLogEnabled { get; set; }

            //internal HttpActivityFeature? HttpActivityFeature;
            //internal HttpMetricsTagsFeature? MetricsTagsFeature;

            public void Reset()
            {
                // Not resetting NiisContext here as we pool it on the Context

                Scope = null;
                //Activity = null;
                //StartLog = null;

                StartTimestamp = 0;
                HasDiagnosticListener = false;
                MetricsEnabled = false;
                EventLogEnabled = false;
                //MetricsTagsFeature?.Reset();
            }
        }
    }
}