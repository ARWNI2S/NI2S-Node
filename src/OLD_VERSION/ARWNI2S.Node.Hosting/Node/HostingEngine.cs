using ARWNI2S.Hosting.Diagnostics;

namespace ARWNI2S.Hosting.Node
{
    internal class HostingEngine : INiisEngine
    {
        private INiisEngine engine;
        private ILogger logger;
        private DiagnosticListener diagnosticListener;
        private ActivitySource activitySource;
        private DistributedContextPropagator propagator;
        private INiisContextFactory contextFactory;
        private HostingEventSource log;
        private HostingMetrics hostingMetrics;

        public HostingEngine(INiisEngine engine, ILogger logger, DiagnosticListener diagnosticListener, ActivitySource activitySource, DistributedContextPropagator propagator, INiisContextFactory contextFactory, HostingEventSource log, HostingMetrics hostingMetrics)
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