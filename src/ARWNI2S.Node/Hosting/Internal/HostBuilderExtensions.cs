using ARWNI2S.Node.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Internal
{
    /// <summary>
    /// Extension methods for configuring the <see cref="IHostBuilder" />.
    /// </summary>
    internal static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures a <see cref="IHostBuilder" /> with defaults for hosting a web app. This should be called
        /// before application specific configuration to avoid it overwriting provided services, configuration sources,
        /// environments, content root, etc.
        /// </summary>
        /// <remarks>
        /// The following defaults are applied to the <see cref="IHostBuilder"/>:
        /// <list type="bullet">
        ///     <item><description>use Kestrel as the web server and configure it using the application's configuration providers</description></item>
        ///     <item><description>configure <see cref="INiisHostEnvironment.NI2SRootFileProvider"/> to include static web assets from projects referenced by the entry assembly during development</description></item>
        ///     <item><description>adds the HostFiltering middleware</description></item>
        ///     <item><description>adds the ForwardedHeaders middleware if ARWNI2S_FORWARDEDHEADERS_ENABLED=true,</description></item>
        ///     <item><description>enable IIS integration</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="builder">The <see cref="IHostBuilder" /> instance to configure.</param>
        /// <param name="configure">The configure callback</param>
        /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
        public static IHostBuilder ConfigureNI2SHostDefaults(this IHostBuilder builder, Action<INiisHostBuilder> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);

            return builder.ConfigureNI2SHostDefaults(configure, _ => { });
        }

        /// <summary>
        /// Configures a <see cref="IHostBuilder" /> with defaults for hosting a web app. This should be called
        /// before application specific configuration to avoid it overwriting provided services, configuration sources,
        /// environments, content root, etc.
        /// </summary>
        /// <remarks>
        /// The following defaults are applied to the <see cref="IHostBuilder"/>:
        /// <list type="bullet">
        ///     <item><description>use Kestrel as the web server and configure it using the application's configuration providers</description></item>
        ///     <item><description>configure <see cref="INiisHostEnvironment.NI2SRootFileProvider"/> to include static web assets from projects referenced by the entry assembly during development</description></item>
        ///     <item><description>adds the HostFiltering middleware</description></item>
        ///     <item><description>adds the ForwardedHeaders middleware if ARWNI2S_FORWARDEDHEADERS_ENABLED=true,</description></item>
        ///     <item><description>enable IIS integration</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="builder">The <see cref="IHostBuilder" /> instance to configure.</param>
        /// <param name="configure">The configure callback</param>
        /// <param name="configureOptions">The delegate that configures the <see cref="HostBuilderOptions"/>.</param>
        /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
        public static IHostBuilder ConfigureNI2SHostDefaults(this IHostBuilder builder, Action<INiisHostBuilder> configure, Action<HostBuilderOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(configure);

            return builder.ConfigureNI2SHost(niisHostBuilder =>
            {
                DefaultNodeServices.ConfigureNI2SDefaults(niisHostBuilder);

                configure(niisHostBuilder);
            }, configureOptions);
        }

        private static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Action<INiisHostBuilder> configure, Action<HostBuilderOptions> configureNiisHostBuilder)
        {
            return builder.ConfigureNI2SHost((hostBuilder, options) => new NI2SHostBuilder(hostBuilder, options), configure, configureNiisHostBuilder);
        }

        private static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Func<IHostBuilder, HostBuilderOptions, INiisHostBuilder> createNI2SHostBuilder, Action<INiisHostBuilder> configure, Action<HostBuilderOptions> configureNiisHostBuilder)
        {
            ArgumentNullException.ThrowIfNull(configure, nameof(configure));
            ArgumentNullException.ThrowIfNull(configureNiisHostBuilder, nameof(configureNiisHostBuilder));

            HostBuilderOptions niisHostBuilderOptions = new();
            configureNiisHostBuilder(niisHostBuilderOptions);
            INiisHostBuilder obj = createNI2SHostBuilder(builder, niisHostBuilderOptions);
            configure(obj);
            builder.ConfigureServices(delegate (HostBuilderContext context, IServiceCollection services)
            {
                services.AddHostedService<NI2SHostService>();
            });
            return builder;
        }

    }
}
