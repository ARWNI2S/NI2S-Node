using System;

namespace NI2S.Node.Statistics
{
    internal class AppEnvironmentStatistics : IAppEnvironmentStatistics
    {
        public long? MemoryUsage => GC.GetTotalMemory(false);
    }
}
