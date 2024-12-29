using ARWNI2S.Engine;
using ARWNI2S.Node.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ARWNI2S.Node
{
    internal class EngineHost : INiisEngine
    {
        private readonly INiisEngine engine;
        private readonly ILogger logger;
        private readonly DiagnosticListener diagnosticListener;
        private readonly ActivitySource activitySource;
        private readonly DistributedContextPropagator propagator;
        private readonly INiisContextFactory contextFactory;
        private readonly object log;
        private readonly HostingMetrics hostingMetrics;

        public EngineHost(INiisEngine engine, ILogger logger, DiagnosticListener diagnosticListener, ActivitySource activitySource, DistributedContextPropagator propagator, INiisContextFactory contextFactory, object log, HostingMetrics hostingMetrics)
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