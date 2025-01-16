using ARWNI2S.Caching;
using ARWNI2S.Cluster.Caching;
using ARWNI2S.Cluster.Extensibility;
using ARWNI2S.Cluster.Maintenance;
using ARWNI2S.Cluster.Nodes;
using ARWNI2S.Collections;
using ARWNI2S.Engine.Caching;
using ARWNI2S.Engine.Data;
using ARWNI2S.Engine.Plugins;
using ARWNI2S.Engine.Threading;
using ARWNI2S.Environment;
using ARWNI2S.Events;
using ARWNI2S.Framework.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Cluster
{
    internal class NI2SClusterModule : ClusterModule
    {
        //public override int Order => NI2SLifecycleStage.BecomeActive;

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            //static cache manager
            var distributedCacheConfig = Settings.Get<DistributedCacheConfig>();

            services.AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>));

            services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
            services.AddScoped<IShortTermCacheManager, PerFrameCacheManager>();

            if (distributedCacheConfig.Enabled)
            {
                switch (distributedCacheConfig.DistributedCacheType)
                {
                    case DistributedCacheType.Memory:
                        services.AddScoped<IStaticCacheManager, MemoryDistributedCacheManager>();
                        services.AddScoped<ICacheKeyService, MemoryDistributedCacheManager>();
                        break;
                    case DistributedCacheType.SqlServer:
                        services.AddScoped<IStaticCacheManager, MsSqlServerCacheManager>();
                        services.AddScoped<ICacheKeyService, MsSqlServerCacheManager>();
                        break;
                    case DistributedCacheType.Redis:
                        services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                        services.AddScoped<IStaticCacheManager, RedisCacheManager>();
                        services.AddScoped<ICacheKeyService, RedisCacheManager>();
                        break;
                    case DistributedCacheType.RedisSynchronizedMemory:
                        services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                        services.AddSingleton<ISynchronizedMemoryCache, RedisSynchronizedMemoryCache>();
                        services.AddSingleton<IStaticCacheManager, SynchronizedMemoryCacheManager>();
                        services.AddScoped<ICacheKeyService, SynchronizedMemoryCacheManager>();
                        break;
                }

                services.AddSingleton<ILocker, DistributedCacheLocker>();
            }
            else
            {
                services.AddSingleton<ILocker, MemoryCacheLocker>();
                services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
                services.AddScoped<ICacheKeyService, MemoryCacheManager>();
            }

            ////work context
            //services.AddScoped<IWorkingContext, DefaultWorkContext>();

            //node context
            services.AddScoped<INodeContext, ClusterNodeContext>();


            //services
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<IMaintenanceTaskService, MaintenanceTaskService>();

            services.AddScoped<INodeService, NodeService>();
            services.AddScoped<INodeMappingService, NodeMappingService>();

            //services.AddScoped<ILogger, DefaultLogger>();

            //services.AddSingleton<IEventPublisher, EventPublisher>();
            //services.AddScoped<ISettingService, SettingService>();

            //register all settings
            var typeFinder = Singleton<ITypeFinder>.Instance;

            var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
            foreach (var setting in settings)
            {
                services.AddScoped(setting, serviceProvider =>
                {
                    var nodeId = (int)(DataSettingsManager.IsDatabaseInstalled()
                        ? serviceProvider.GetRequiredService<INodeContext>().GetCurrentNode()?.Id ?? 0
                        : 0);

                    return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting, nodeId).Result;
                });
            }

            ////installation service
            //services.AddScoped<IInstallationService, InstallationService>();

            ////schedule tasks
            //services.AddSingleton<ITaskScheduler, TaskScheduler>();
            services.AddTransient<IMaintenanceTaskRunner, MaintenanceTaskRunner>();

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IEventConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IEventConsumer<>)))
                    services.AddScoped(findInterface, consumer);

            //NodeConfig nodeConfig = Singleton<NI2SSettings>.Instance.Get<NodeConfig>();
            //ClusterConfig clusterConfig = Singleton<NI2SSettings>.Instance.Get<ClusterConfig>();

            //if (nodeConfig.Type == NodeType.Interactive)
            //{
            //}



            //services.AddOrleansClient(clientBuilder =>
            //{
            //    clientBuilder.Configure<ClusterOptions>(options =>
            //    {
            //        options.ClusterId = clusterConfig.ClusterId;
            //        options.ServiceId = clusterConfig.ServiceId;
            //    });

            //    switch (clusterConfig.Storage)
            //    {
            //        case StorageType.Local:
            //            {
            //                clientBuilder.UseLocalhostClustering();
            //                break;
            //            }
            //        case StorageType.AzureStorage:
            //            {
            //                clientBuilder.UseAzureTables
            //                break;
            //            }
            //        case StorageType.AdoNet:
            //            {
            //                clientBuilder.UseSql
            //                break;
            //            }
            //        case StorageType.DynamoDB:
            //            {
            //                clientBuilder.UseDynamoDB
            //                break;
            //            }
            //        case StorageType.ServiceFabric:
            //            {
            //                clientBuilder.UseServiceFabric
            //                break;
            //            }
            //        case StorageType.Consul:
            //            {
            //                clientBuilder.UseConsul
            //                break;
            //            }
            //        case StorageType.ZooKeeper:
            //            {
            //                clientBuilder.UseZooKeeper
            //                break;
            //            }
            //        default:
            //            break;
            //    }
            //});
        }
    }
}
