using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Clustering
{
    public class ClusterStartup : INiisStartup
    {
        public int Order => 150;

        public void ConfigureEngine(IEngineBuilder engineBuilder)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
