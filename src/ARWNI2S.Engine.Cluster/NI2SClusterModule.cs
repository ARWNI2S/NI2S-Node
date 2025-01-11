using ARWNI2S.Cluster.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Cluster
{
    internal class NI2SClusterModule : ClusterModule
    {
        //public override int Order => NI2SLifecycleStage.BecomeActive;

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            //services.AddDistributionServices();










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
