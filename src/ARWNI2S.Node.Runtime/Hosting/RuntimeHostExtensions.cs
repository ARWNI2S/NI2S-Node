﻿using ARWNI2S.Cluster;

namespace ARWNI2S.Node.Hosting
{
    internal static class RuntimeHostExtensions
    {
        public static Task StartAsync(this IClusterNode node, EngineHost engineHost, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
