using ARWNI2S.Engine.Cluster.Hosting;
using ARWNI2S.Engine.Core;
using ARWNI2S.Engine.Core.Builder;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Engine.Plugins;
using ARWNI2S.Hosting;
using ARWNI2S.Hosting.Builder;
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
                    services.AddRelayer();
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
                services.AddContextAccessor();
                //initialize engine and plugins
                services.AddNI2SCore().InitializePlugins(hostingContext.Configuration);

                if (configureCluster == null)
                {
                    services.AddRelayerCore();
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