﻿namespace ARWNI2S.Clustering.Monitor
{
    public partial class ClusterMonitorService : IClusterMonitorService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}