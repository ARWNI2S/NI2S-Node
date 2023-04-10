using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Hosting.Infrastructure;
using System;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Contains extensions for an <see cref="IHostBuilder"/>.
    /// </summary>
    public static class GenericHostNodeHostBuilderExtensions
    {
        /// <summary>
        /// Adds and configures an ASP.NET Core web application.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/> to add the <see cref="INodeHostBuilder"/> to.</param>
        /// <param name="configure">The delegate that configures the <see cref="INodeHostBuilder"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureNodeHost(this IHostBuilder builder, Action<INodeHostBuilder> configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            return builder.ConfigureNodeHost(configure, _ => { });
        }

        /// <summary>
        /// Adds and configures an ASP.NET Core web application.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/> to add the <see cref="INodeHostBuilder"/> to.</param>
        /// <param name="configure">The delegate that configures the <see cref="INodeHostBuilder"/>.</param>
        /// <param name="configureNodeHostBuilder">The delegate that configures the <see cref="NodeHostBuilderOptions"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        /* 004 */
        public static IHostBuilder ConfigureNodeHost(this IHostBuilder builder, Action<INodeHostBuilder> configure, Action<NodeHostBuilderOptions> configureNodeHostBuilder)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            if (configureNodeHostBuilder is null)
            {
                throw new ArgumentNullException(nameof(configureNodeHostBuilder));
            }

            // Light up custom implementations namely ConfigureHostBuilder which throws.
            if (builder is ISupportsConfigureNodeHost supportsConfigureNodeHost)
            {
                return supportsConfigureNodeHost.ConfigureNodeHost(configure, configureNodeHostBuilder);
            }

            var nodeHostBuilderOptions = new NodeHostBuilderOptions();
            configureNodeHostBuilder(nodeHostBuilderOptions);
            var nodehostBuilder = new GenericNodeHostBuilder(builder, nodeHostBuilderOptions);
            configure(nodehostBuilder);
            builder.ConfigureServices((context, services) => /* 037 */services.AddHostedService<GenericNodeHostService>());
            return builder;
        }
    }
}
