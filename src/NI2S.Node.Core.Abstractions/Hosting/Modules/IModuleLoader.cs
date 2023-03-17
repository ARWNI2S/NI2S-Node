using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NI2S.Node.Hosting.Modules
{
    public interface IModuleLoader
    {
        LoadingPhase LoadingPhase { get; }

        void Configure(INodeHostBuilder builder);
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}
