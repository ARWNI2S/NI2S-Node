// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using System;

namespace NI2S.Node.Hosting.Extensions
{
    internal sealed class TrimmingAppContextSwitches
    {
        private const string EnsureJsonTrimmabilityKey = "Microsoft.AspNetCore.EnsureJsonTrimmability";

        internal static bool EnsureJsonTrimmability { get; } = AppContext.TryGetSwitch(EnsureJsonTrimmabilityKey, out var enabled) && enabled;
    }
}