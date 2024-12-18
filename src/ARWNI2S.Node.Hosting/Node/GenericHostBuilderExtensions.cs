using ARWNI2S.Hosting.Configuration;

namespace ARWNI2S.Hosting.Node
{
    /// <summary>
    /// Extension methods for configuring the <see cref="IHostBuilder" />.
    /// </summary>
    public static class GenericHostBuilderExtensions
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
        ///     <item><description>configure <see cref="INiisHostEnvironment.NodeRootFileProvider"/> to include static web assets from projects referenced by the entry assembly during development</description></item>
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
        ///     <item><description>configure <see cref="INiisHostEnvironment.NodeRootFileProvider"/> to include static web assets from projects referenced by the entry assembly during development</description></item>
        ///     <item><description>adds the HostFiltering middleware</description></item>
        ///     <item><description>adds the ForwardedHeaders middleware if ARWNI2S_FORWARDEDHEADERS_ENABLED=true,</description></item>
        ///     <item><description>enable IIS integration</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="builder">The <see cref="IHostBuilder" /> instance to configure.</param>
        /// <param name="configure">The configure callback</param>
        /// <param name="configureOptions">The delegate that configures the <see cref="NI2SHostBuilderOptions"/>.</param>
        /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
        public static IHostBuilder ConfigureNI2SHostDefaults(this IHostBuilder builder, Action<INiisHostBuilder> configure, Action<NI2SHostBuilderOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(configure);

            return builder.ConfigureNI2SHost(niisHostBuilder =>
            {
                NI2SHost.ConfigureNI2SDefaults(niisHostBuilder);

                configure(niisHostBuilder);
            }, configureOptions);
        }
    }
}
