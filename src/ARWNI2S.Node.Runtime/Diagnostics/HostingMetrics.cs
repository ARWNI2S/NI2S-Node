// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.Metrics;

namespace ARWNI2S.Node.Diagnostics
{
    internal class HostingMetrics
    {
        public const string MeterName = "ARWNI2S.Node.Hosting";

        private readonly Meter _meter;

        public HostingMetrics(IMeterFactory meterFactory)
        {
            _meter = meterFactory.Create(MeterName);
        }
    }
}