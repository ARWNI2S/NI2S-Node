using ARWNI2S.Engine.Hosting;
using ARWNI2S.Engine.Cluster.Hosting;
using ARWNI2S.Node.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal class DefaultNodeServices
    {
        internal static void ConfigureNI2SDefaults(INiisHostBuilder builder)
        {
            builder.UseLocalAssets();

            ConfigureNode(
                builder.UseNI2SEngine()
                .UseClustering(ConfigureCluster)
                .UseNI2SNodeIntegration(),
                (services, config) =>
                {
                    //services.AddRouting()
                });

        }

        private static void ConfigureCluster(NI2SHostBuilderContext builderContext, ClusterNodeOptions options)
        {
            //options.Configure(builderContext.Configuration.GetSection("Kestrel"), reloadOnChange: true);
        }

        private static INiisHostBuilder ConfigureNode(INiisHostBuilder builder, Action<IServiceCollection, IConfiguration> configureCluster)
        {
            builder.ConfigureServices((hostingContext, services) => //5
            {


                if (configureCluster == null)
                {
                    //services.AddRoutingCore()
                }
                else
                {
                    configureCluster(services, hostingContext.Configuration);
                }
            });

            return builder;
        }

    }
}