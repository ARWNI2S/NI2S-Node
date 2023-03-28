using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration;
using System;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Extension methods for configuring the <see cref="IHostBuilder" />.
    /// </summary>
    public static class GenericHostBuilderExtensions
    {
        /// <summary>
        /// Configures a <see cref="IHostBuilder" /> with defaults for hosting a NI2S node. This should be called
        /// before specific configuration to avoid it overwriting provided services, configuration sources,
        /// environments, content root, etc.
        /// </summary>
        /// <remarks>
        /// The following defaults are applied to the <see cref="IHostBuilder"/>:
        /// <list type="bullet">
        /// TODO: MODIFY ALL
        ///     <item><description>use Kestrel as the web server and configure it using the application's configuration providers</description></item>
        ///     <item><description>configure <see cref="INodeHostEnvironment.AssetsRootFileProvider"/> to include static web assets from projects referenced by the entry assembly during development</description></item>
        ///     <item><description>adds the HostFiltering middleware</description></item>
        ///     <item><description>adds the ForwardedHeaders middleware if NETCORE_FORWARDEDHEADERS_ENABLED=true,</description></item>
        ///     <item><description>enable IIS integration</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="builder">The <see cref="IHostBuilder" /> instance to configure.</param>
        /// <param name="configure">The configure callback</param>
        /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
        public static IHostBuilder ConfigureNodeHostDefaults(this IHostBuilder builder, Action<INodeHostBuilder> configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            return builder.ConfigureNodeHostDefaults(configure, _ => { });
        }

        /// <summary>
        /// Configures a <see cref="IHostBuilder" /> with defaults for hosting a NI2S node. This should be called
        /// before specific configuration to avoid it overwriting provided services, configuration sources,
        /// environments, content root, etc.
        /// </summary>
        /// <remarks>
        /// The following defaults are applied to the <see cref="IHostBuilder"/>:
        /// <list type="bullet">
        /// TODO: MODIFY ALL
        ///     <item><description>use Kestrel as the web server and configure it using the application's configuration providers</description></item>
        ///     <item><description>configure <see cref="INodeHostEnvironment.AssetsRootFileProvider"/> to include static web assets from projects referenced by the entry assembly during development</description></item>
        ///     <item><description>adds the HostFiltering middleware</description></item>
        ///     <item><description>adds the ForwardedHeaders middleware if NETCORE_FORWARDEDHEADERS_ENABLED=true,</description></item>
        ///     <item><description>enable IIS integration</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="builder">The <see cref="IHostBuilder" /> instance to configure.</param>
        /// <param name="configure">The configure callback</param>
        /// <param name="configureOptions">The delegate that configures the <see cref="NodeHostBuilderOptions"/>.</param>
        /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
        public static IHostBuilder ConfigureNodeHostDefaults(this IHostBuilder builder, Action<INodeHostBuilder> configure, Action<NodeHostBuilderOptions> configureOptions)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            return builder.ConfigureNodeHost(nodeHostBuilder =>
            {
                Node.NodeHost.ConfigureNodeDefaults(nodeHostBuilder);

                configure(nodeHostBuilder);
            }, configureOptions);
        }
    }
}
