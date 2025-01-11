// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Cluster.Diagnostics
{
    internal sealed class ConnectionMetricsContext
    {
        //public required BaseConnectionContext ConnectionContext { get; init; }
        public bool CurrentConnectionsCounterEnabled { get; init; }
        public bool ConnectionDurationEnabled { get; init; }
        public bool QueuedConnectionsCounterEnabled { get; init; }
        public bool QueuedRequestsCounterEnabled { get; init; }
        public bool CurrentUpgradedRequestsCounterEnabled { get; init; }
        public bool CurrentTlsHandshakesCounterEnabled { get; init; }

        //public ConnectionEndReason? ConnectionEndReason { get; set; }
    }
}