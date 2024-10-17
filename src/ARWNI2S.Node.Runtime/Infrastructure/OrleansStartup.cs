using ARWNI2S.Infrastructure;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

namespace ARWNI2S.Narrator.Framework.Infrastructure
{
    public class OrleansStartup : INI2SStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //narrator node
            var ni2sSettings = Singleton<NI2SSettings>.Instance;
            var clusterConfig = ni2sSettings.Get<ClusterConfig>();

            if (clusterConfig.UseFrontline)
            {
                services.AddOrleansClient(client =>
                {
                    client = client.Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = clusterConfig.ClusterId;
                        options.ServiceId = clusterConfig.ServiceId;
                    });

                    if (!clusterConfig.IsDevelopment)
                    {

                    }
                    else
                    {
                        client.UseLocalhostClustering();
                    }
                });
            }
        }

        public void Configure(IHost application)
        {
            // Orleans normalmente no necesita configuraciones adicionales aquí
        }

        public int Order => 150;    // Puedes ajustar el orden según sea necesario
    }
}
