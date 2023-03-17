using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting;
using NI2S.Node.Hosting.Modules;

namespace NI2S.Node.Data
{
    public sealed class DataModuleLoader : IModuleLoader
    {
        public LoadingPhase LoadingPhase => LoadingPhase.InitFirst;

        public void Configure(INodeHostBuilder builder)
        {
            throw new System.NotImplementedException();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}
