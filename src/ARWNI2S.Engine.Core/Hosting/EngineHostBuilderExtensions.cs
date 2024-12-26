using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Extensibility;
using ARWNI2S.Hosting.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Hosting
{
    public static class EngineHostBuilderExtensions
    {
        public static INiisHostBuilder UseNI2SEngine(this INiisHostBuilder builder)
        {
            builder.ConfigureServices((hostingContext, services) => //2
            {
                services.AddSingleton<IEngineModuleManager, NI2SModuleManager>();

            });

            return builder;
        }

        public static INiisHostBuilder UseNI2SNodeIntegration(this INiisHostBuilder builder)
        {
            builder.ConfigureServices((hostingContext, services) => //4
            {


            });

            return builder;
        }
    }
}
