// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Cluster.Configuration;
using System.Diagnostics;

namespace ARWNI2S.Cluster
{
    // Ideally this type should be readonly and initialized with a constructor.
    // Tests use TestServiceContext which inherits from this type and sets properties.
    // Changing this type would be a lot of work.
    internal class ServiceContext
    {
        //public ClusterTrace Log { get; set; } = default!;

        //public PipeScheduler Scheduler { get; set; } = default!;

        //public IHttpParser<Http1ParsingHandler> HttpParser { get; set; } = default!;

        //public TimeProvider TimeProvider { get; set; } = default!;

        //public DateHeaderValueManager DateHeaderValueManager { get; set; } = default!;

        //public ConnectionManager ConnectionManager { get; set; } = default!;

        //public Heartbeat Heartbeat { get; set; } = default!;

        public ClusterNodeOptions NodeOptions { get; set; } = default!;

        public DiagnosticSource DiagnosticSource { get; set; }

        //public ClusterNodeMetrics Metrics { get; set; } = default!;
    }
}