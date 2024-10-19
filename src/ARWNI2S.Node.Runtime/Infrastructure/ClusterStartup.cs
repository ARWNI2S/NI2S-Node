using ARWNI2S.Infrastructure;
using ARWNI2S.Runtime.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Runtime.Infrastructure
{
    public class ClusterStartup : INI2SStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddNI2SFrontline();

            services.AddNI2SClustering();
        }

        public void Configure(IHost application)
        {
            application.UseNI2SClustering();
        }

        public int Order => 150;    // Puedes ajustar el orden según sea necesario
    }
}
