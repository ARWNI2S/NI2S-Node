using ARWNI2S.Hosting.Node;
using ARWNI2S.Node.Hosting;
using ARWNI2S.Node.Hosting.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Builder.Extensions
{
    public static class GenericHostINiisHostBuilderExtensions
    {
        public static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Action<INiisHostBuilder> configure, Action<NI2SHostBuilderOptions> configureNiisHostBuilder)
        {
            return builder.ConfigureNI2SHost((hostBuilder, options) => new GenericNI2SHostBuilder(hostBuilder, options), configure, configureNiisHostBuilder);
        }

        private static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Func<IHostBuilder, NI2SHostBuilderOptions, INiisHostBuilder> createNI2SHostBuilder, Action<INiisHostBuilder> configure, Action<NI2SHostBuilderOptions> configureNiisHostBuilder)
        {
            ArgumentNullException.ThrowIfNull(configure, nameof(configure));
            ArgumentNullException.ThrowIfNull(configureNiisHostBuilder, nameof(configureNiisHostBuilder));

            NI2SHostBuilderOptions niisHostBuilderOptions = new();
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
