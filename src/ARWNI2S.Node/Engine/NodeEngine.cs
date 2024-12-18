using ARWNI2S.Engine;
using ARWNI2S.Engine.Infrastructure;
using ARWNI2S.Node.Hosting.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ARWNI2S.Node.Engine
{
    internal sealed class NodeEngine : INiisEngine
    {
        private INiisEngine engine;
        private ILogger logger;
        private DiagnosticListener diagnosticListener;
        private ActivitySource activitySource;
        private DistributedContextPropagator propagator;
        private INiisContextFactory contextFactory;
        private HostingEventSource log;
        private HostingMetrics hostingMetrics;

        public NodeEngine(INiisEngine engine, ILogger logger, DiagnosticListener diagnosticListener, ActivitySource activitySource, DistributedContextPropagator propagator, INiisContextFactory contextFactory, HostingEventSource log, HostingMetrics hostingMetrics)
        {
            this.engine = engine;
            this.logger = logger;
            this.diagnosticListener = diagnosticListener;
            this.activitySource = activitySource;
            this.propagator = propagator;
            this.contextFactory = contextFactory;
            this.log = log;
            this.hostingMetrics = hostingMetrics;
        }
    }
}
