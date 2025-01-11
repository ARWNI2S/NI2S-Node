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