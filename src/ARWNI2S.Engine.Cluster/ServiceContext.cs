using ARWNI2S.Cluster.Connection;
using ARWNI2S.Cluster.Diagnostics;
using ARWNI2S.Cluster.Environment;
using ARWNI2S.Cluster.Hosting;
using ARWNI2S.Engine.Cluster.Logging;
using System.Diagnostics;
using System.IO.Pipelines;

namespace ARWNI2S.Cluster
{
    // Ideally this type should be readonly and initialized with a constructor.
    // Tests use TestServiceContext which inherits from this type and sets properties.
    // Changing this type would be a lot of work.
    internal class ServiceContext
    {
        public ClusterTrace Log { get; set; } = default!;

        public PipeScheduler Scheduler { get; set; } = default!;

        //public IHttpParser<Http1ParsingHandler> HttpParser { get; set; } = default!;

        public TimeProvider TimeProvider { get; set; } = default!;

        //public DateHeaderValueManager DateHeaderValueManager { get; set; } = default!;

        public ConnectionManager ConnectionManager { get; set; } = default!;

        public Heartbeat Heartbeat { get; set; } = default!;

        public ClusterNodeOptions NodeOptions { get; set; } = default!;

        public DiagnosticSource DiagnosticSource { get; set; }

        public ClusterNodeMetrics Metrics { get; set; } = default!;
    }
}