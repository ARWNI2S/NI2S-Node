using ARWNI2S.Cluster;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal static class RuntimeHostExtensions
    {
        internal static Task StartAsync(this IClusterNode node, EngineHost engineHost, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
