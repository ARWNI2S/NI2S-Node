using ARWNI2S.Clustering.Configuration.Options;
using ARWNI2S.Node.Builder;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;

namespace ARWNI2S.Clustering.Hosting.Extensions
{
    public static class NodeHostBuilderExtensions
    {
        public static INodeHostBuilder UseClustering(this INodeHostBuilder builder, Action<NodeHostBuilderContext, ClusteringServerOptions> configure)
        {
            return builder.UseClusteringCore().ConfigureClustering(configure);
        }

        public static INodeHostBuilder UseClusteringCore(this INodeHostBuilder builder)
        {
            return builder;
        }

        public static INodeHostBuilder ConfigureClustering(this INodeHostBuilder builder, Action<NodeHostBuilderContext, ClusteringServerOptions> configure)
        {
            return builder;
        }

        public static INodeHostBuilder UseOrleans(this INodeHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Configuración compartida de Orleans
                services.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "SimulationCluster";
                    options.ServiceId = "SimulationService";
                });

                //// Configuración de serialización
                //services.Configure<SerializationProviderOptions>(options =>
                //{
                //    options.AddSerializer<JsonSerializer>();
                //});

                //// Partes de la aplicación
                //services.AddSingleton<IApplicationPartManager>(provider =>
                //{
                //    var manager = new ApplicationPartManager();
                //    manager.AddApplicationPart(typeof(ISimulationService).Assembly).WithReferences();
                //    return manager;
                //});
            });


            return builder;
        }

        public static INodeHostBuilder UseOrleansIntegration(this INodeHostBuilder builder)
        {
            return builder;
        }
    }
}
