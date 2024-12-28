using ARWNI2S.Engine;

namespace ARWNI2S.Cluster
{
    public static class ServiceClusterNodeExtensions
    {
        public static async Task StartAsync(this IClusterNode clusterNode, INiisEngine engine, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

    }
}
