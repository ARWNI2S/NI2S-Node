using ARWNI2S.Hosting.Configuration;

namespace ARWNI2S.Hosting.Node
{
    public static class GenericHostNI2SHostBuilderExtensions
    {
        public static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Action<INiisHostBuilder> configure, Action<NI2SHostBuilderOptions> configureNiisHostBuilder)
        {
            return builder.ConfigureNI2SHost((hostBuilder, options) => new GenericNI2SHostBuilder(hostBuilder, options), configure, configureNiisHostBuilder);
        }

        private static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Func<IHostBuilder, NI2SHostBuilderOptions, INiisHostBuilder> createNI2SHostBuilder, Action<INiisHostBuilder> configure, Action<NI2SHostBuilderOptions> configureNiisHostBuilder)
        {
            ArgumentNullException.ThrowIfNull(configure, "configure");
            ArgumentNullException.ThrowIfNull(configureNiisHostBuilder, "configureNiisHostBuilder");
            //if (builder is ISupportsConfigureNiisHost supportsConfigureNI2SHost)
            //{
            //    return supportsConfigureNI2SHost.ConfigureNI2SHost(configure, configureNiisHostBuilder);
            //}
            NI2SHostBuilderOptions niisHostBuilderOptions = new NI2SHostBuilderOptions();
            configureNiisHostBuilder(niisHostBuilderOptions);
            INiisHostBuilder obj = createNI2SHostBuilder(builder, niisHostBuilderOptions);
            configure(obj);
            builder.ConfigureServices(delegate (HostBuilderContext context, IServiceCollection services)
            {
                services.AddHostedService<GenericNI2SHostService>();
            });
            return builder;
        }
    }
}
