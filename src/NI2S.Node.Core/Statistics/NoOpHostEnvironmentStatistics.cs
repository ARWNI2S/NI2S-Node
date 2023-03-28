using Microsoft.Extensions.Logging;
using NI2S.Node.Statistics;

namespace NI2S.Node.Runtime
{
    internal class NoOpHostEnvironmentStatistics : IHostEnvironmentStatistics
    {
        public long? TotalPhysicalMemory => null;

        public float? CpuUsage => null;

        public long? AvailableMemory => null;

        public NoOpHostEnvironmentStatistics(ILogger<NoOpHostEnvironmentStatistics> logger)
        {
            logger.LogWarning(
                (int)ErrorCode.PerfCounterNotRegistered,
                "No implementation of IHostEnvironmentStatistics was found. Load shedding will not work yet");
        }
    }
}
