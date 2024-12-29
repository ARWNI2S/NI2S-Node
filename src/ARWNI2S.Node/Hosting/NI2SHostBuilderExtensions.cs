using ARWNI2S.Engine.Builder;
using ARWNI2S.Hosting;
using ARWNI2S.Hosting.Builder;
using ARWNI2S.Node.Assets;
using ARWNI2S.Node.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Node.Hosting
{
    public static class NI2SHostBuilderExtensions
    {
        internal static INiisHostBuilder Configure(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, IEngineBuilder> configureEngine)
        {
            ArgumentNullException.ThrowIfNull(configureEngine, nameof(configureEngine));
            if (hostBuilder is NI2SHostBuilder builder)
                return builder.ConfigureEngine(configureEngine);

            throw new NotSupportedException("hostBuilder is not NI2SHostBuilder");
        }

        public static INiisHostBuilder UseDefaultServiceProvider(this INiisHostBuilder hostBuilder, Action<ServiceProviderOptions> configure)
        {
            return hostBuilder.UseDefaultServiceProvider((NI2SHostBuilderContext context, ServiceProviderOptions options) => configure(options));
        }

        internal static INiisHostBuilder UseDefaultServiceProvider(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, ServiceProviderOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(configure, nameof(configure));
            if (hostBuilder is NI2SHostBuilder builder)
                return builder.UseDefaultServiceProvider(configure);

            throw new NotSupportedException("hostBuilder is not NI2SHostBuilder");
        }

        public static INiisHostBuilder ConfigureAppConfiguration(this INiisHostBuilder hostBuilder, Action<IConfigurationBuilder> configureDelegate)
        {
            Action<IConfigurationBuilder> configureDelegate2 = configureDelegate;
            return hostBuilder.ConfigureNI2SConfiguration(delegate (NI2SHostBuilderContext context, IConfigurationBuilder builder)
            {
                configureDelegate2(builder);
            });
        }

        public static INiisHostBuilder ConfigureLogging(this INiisHostBuilder hostBuilder, Action<ILoggingBuilder> configureLogging)
        {
            Action<ILoggingBuilder> configureLogging2 = configureLogging;
            return hostBuilder.ConfigureServices(delegate (IServiceCollection collection)
            {
                collection.AddLogging(configureLogging2);
            });
        }

        public static INiisHostBuilder ConfigureLogging(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, ILoggingBuilder> configureLogging)
        {
            Action<NI2SHostBuilderContext, ILoggingBuilder> configureLogging2 = configureLogging;
            return hostBuilder.ConfigureServices(delegate (NI2SHostBuilderContext context, IServiceCollection collection)
            {
                collection.AddLogging(delegate (ILoggingBuilder builder)
                {
                    configureLogging2(context, builder);
                });
            });
        }

        public static INiisHostBuilder UseLocalAssets(this INiisHostBuilder builder)
        {
            builder.ConfigureNI2SConfiguration(delegate (NI2SHostBuilderContext context, IConfigurationBuilder configBuilder) //2
            {
                NodeAssetsLoader.UseLocalAssets(context.HostingEnvironment, context.Configuration);
            });
            return builder;
        }
    }
}
