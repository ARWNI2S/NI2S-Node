using System.Diagnostics.Metrics;

namespace NI2S.Node.Runtime;

internal static class Instruments
{
    internal static readonly Meter Meter = new("Microsoft.Orleans");
}
