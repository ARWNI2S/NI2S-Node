using ARWNI2S.Extensibility;
using ARWNI2S.Hosting.Builder;
using ARWNI2S.Node.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting
{
    public static class EngineHostBuilderExtensions
    {
        public static INiisHostBuilder UseNI2SNodeIntegration(this INiisHostBuilder builder)
        {
            builder.ConfigureServices((hostingContext, services) => //4
            {
                services.AddSingleton<IModuleManager, RuntimeModuleManager>();


            });

            return builder;
        }
    }
}
