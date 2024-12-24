using ARWNI2S.Node.Builder;

namespace ARWNI2S.Engine.Hosting
{
    public static class EngineHostBuilderExtensions
    {
        public static INiisHostBuilder UseNI2SEngine(this INiisHostBuilder builder)
        {
            builder.ConfigureServices((hostingContext, services) => //2
            {


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
