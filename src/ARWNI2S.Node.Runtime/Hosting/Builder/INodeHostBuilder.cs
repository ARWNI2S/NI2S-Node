using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Runtime.Hosting.Builder
{
    public interface INodeHostBuilder
    {
        INodeHost Build();
        INodeHostBuilder ConfigureAppConfiguration(Action<NodeHostBuilderContext, IConfigurationBuilder> configureDelegate);
        INodeHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);
        INodeHostBuilder ConfigureServices(Action<NodeHostBuilderContext, IServiceCollection> configureServices);
        string GetSetting(string key);
        INodeHostBuilder UseSetting(string key, string value);
    }
}