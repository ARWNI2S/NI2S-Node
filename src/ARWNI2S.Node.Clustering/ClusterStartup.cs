﻿using ARWNI2S.Clustering.Configuration;
using ARWNI2S.Clustering.Nodes.Configuration;
using ARWNI2S.Clustering.Services;
using ARWNI2S.Clustering.Services.ScheduleTasks;
using ARWNI2S.Configuration;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Clustering
{
    public class ClusterStartup : INiisStartup
    {
        public int Order => StartupStage.ClusterStartup;

        public void ConfigureEngine(IEngineBuilder engineBuilder)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var ni2sSettings = Singleton<NI2SSettings>.Instance;
            var nodeConfig = ni2sSettings.Get<NodeConfig>();
            var clusterConfig = ni2sSettings.Get<ClusterConfig>();

            services.AddScoped<IClusteringService, ClusteringService>();
            services.AddScoped<INodeMappingService, NodeMappingService>();

            //services.AddSingleton<ClusterManager>();

            //services.AddHostedService<NodeHealthMonitorService>();

            services.AddSingleton<IClusterTaskScheduler, ClusterTaskScheduler>();

        }
    }
}