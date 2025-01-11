// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Cluster;

namespace ARWNI2S.Node.Hosting
{
    internal static class RuntimeHostExtensions
    {
        internal static Task StartAsync(this IClusterNode node, EngineHost engineHost, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
