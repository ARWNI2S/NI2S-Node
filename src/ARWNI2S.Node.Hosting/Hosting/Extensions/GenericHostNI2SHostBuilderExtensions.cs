using ARWNI2S.Node.Hosting.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Extensions
{
    /// <summary>
    /// Contains extensions for an <see cref="IHostBuilder"/>.
    /// </summary>
    public static class GenericHostNI2SHostBuilderExtensions
    {
        /// <summary>
        /// Adds and configures an ASP.NET Core web application.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/> to add the <see cref="INI2SHostBuilder"/> to.</param>
        /// <param name="configure">The delegate that configures the <see cref="INI2SHostBuilder"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Action<INI2SHostBuilder> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);

            return builder.ConfigureNI2SHost(configure, _ => { });
        }

        /// <summary>
        /// Adds and configures an ASP.NET Core web application.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/> to add the <see cref="INI2SHostBuilder"/> to.</param>
        /// <param name="configure">The delegate that configures the <see cref="INI2SHostBuilder"/>.</param>
        /// <param name="configureNI2SHostBuilder">The delegate that configures the <see cref="NI2SHostBuilderOptions"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureNI2SHost(this IHostBuilder builder, Action<INI2SHostBuilder> configure, Action<NI2SHostBuilderOptions> configureNI2SHostBuilder)
        {
            return ConfigureNI2SHost(
                builder,
                static (hostBuilder, options) => new GenericNI2SHostBuilder(hostBuilder, options),
                configure,
                configureNI2SHostBuilder);
        }

        /// <summary>
        /// Adds and configures an ASP.NET Core web application with minimal dependencies.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/> to add the <see cref="INI2SHostBuilder"/> to.</param>
        /// <param name="configure">The delegate that configures the <see cref="INI2SHostBuilder"/>.</param>
        /// <param name="configureNI2SHostBuilder">The delegate that configures the <see cref="NI2SHostBuilderOptions"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureMinimalNI2SHost(this IHostBuilder builder, Action<INI2SHostBuilder> configure, Action<NI2SHostBuilderOptions> configureNI2SHostBuilder)
        {
            return ConfigureNI2SHost(
                builder,
                static (hostBuilder, options) => new MinimalNI2SHostBuilder(hostBuilder, options),
                configure,
                configureNI2SHostBuilder);
        }

        private static IHostBuilder ConfigureNI2SHost(
            this IHostBuilder builder,
            Func<IHostBuilder, NI2SHostBuilderOptions, INI2SHostBuilder> createNI2SHostBuilder,
            Action<INI2SHostBuilder> configure,
            Action<NI2SHostBuilderOptions> configureNI2SHostBuilder)
        {
            ArgumentNullException.ThrowIfNull(configure);
            ArgumentNullException.ThrowIfNull(configureNI2SHostBuilder);

            // Light up custom implementations namely ConfigureHostBuilder which throws.
            if (builder is ISupportsConfigureNI2SHost supportsConfigureNI2SHost)
            {
                return supportsConfigureNI2SHost.ConfigureNI2SHost(configure, configureNI2SHostBuilder);
            }

            var webHostBuilderOptions = new NI2SHostBuilderOptions();
            configureNI2SHostBuilder(webHostBuilderOptions);
            var webhostBuilder = createNI2SHostBuilder(builder, webHostBuilderOptions);
            configure(webhostBuilder);
            builder.ConfigureServices((context, services) => services.AddHostedService<GenericNI2SHostService>());
            return builder;
        }
    }
}
