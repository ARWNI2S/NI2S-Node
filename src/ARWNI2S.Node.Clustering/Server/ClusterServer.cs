﻿using ARWNI2S.Clustering.Data;
using ARWNI2S.Clustering.Nodes.Features;

namespace ARWNI2S.Clustering.Server
{
    internal class ClusterServer : IClusterServer
    {
        private readonly NI2SSettings _settings;

        public NodeFeatures Features { get; }

        public Guid NodeId => Node.NodeId;
        public NI2SNode Node { get; protected set; }

        IFeatureCollection IClusterServer.Features => Features;
        INiisNode IClusterServer.Node => Node;

        public ClusterServer(NI2SSettings settings)
        {
            _settings = settings;
        }

        public async Task StartAsync(INiisEngine ni2sEngine, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
