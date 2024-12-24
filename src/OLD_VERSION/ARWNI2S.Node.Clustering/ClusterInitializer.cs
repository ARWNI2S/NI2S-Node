using ARWNI2S.Clustering.Monitor;
using ARWNI2S.Clustering.Nodes.Configuration;
using ARWNI2S.Clustering.Services;
using ARWNI2S.Clustering.Services.ScheduleTasks;

namespace ARWNI2S.Clustering
{
    public class ClusterInitializer : IInitializer
    {
        public int Order => InitStage.ClusterInit;

        public void ConfigureEngine(IEngineBuilder engineBuilder)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var ni2sSettings = Singleton<NI2SSettings>.Instance;

            services.AddScoped<IClusteringService, ClusteringService>();
            services.AddScoped<INodeMappingService, NodeMappingService>();
            services.AddScoped<IScheduleTaskService, ScheduleTaskService>();

            if (ni2sSettings.Get<NodeConfig>().NodeType != NodeType.Narrator)
            {
                services.AddSingleton<IClusterMonitorService, ClusterMonitorService>();
            }

            services.AddSingleton<NI2SClusterManager>();

            services.AddSingleton<IClusterTaskScheduler, ClusterTaskScheduler>();


            services.AddHostedService(sp => sp.GetRequiredService<IClusterMonitorService>());
        }
    }
}
