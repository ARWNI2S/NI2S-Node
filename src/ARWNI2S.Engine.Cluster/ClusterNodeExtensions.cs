namespace ARWNI2S.Engine.Cluster
{
    public static class ServiceClusterNodeExtensions
    {
        public static async Task StartAsync(this IClusterNode clusterNode, INiisEngine engine, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

    }
}
