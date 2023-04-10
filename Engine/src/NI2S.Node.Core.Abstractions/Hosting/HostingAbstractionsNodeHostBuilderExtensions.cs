using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Core.Infrastructure;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Contains extension methods for configuring the <see cref="INodeHostBuilder" />.
    /// </summary>
    public static class HostingAbstractionsNodeHostBuilderExtensions
    {
        /// <summary>
        /// Use the given configuration settings on the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing settings to be used.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseConfiguration(this INodeHostBuilder hostBuilder, IConfiguration configuration)
        {
            foreach (var setting in configuration.AsEnumerable(makePathsRelative: true))
            {
                hostBuilder.UseSetting(setting.Key, setting.Value);
            }

            return hostBuilder;
        }

        /// <summary>
        /// Set whether startup errors should be captured in the configuration settings of the web host.
        /// When enabled, startup exceptions will be caught and an error page will be returned. If disabled, startup exceptions will be propagated.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="captureStartupErrors"><c>true</c> to use startup error page; otherwise <c>false</c>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder CaptureStartupErrors(this INodeHostBuilder hostBuilder, bool captureStartupErrors)
        {
            return hostBuilder.UseSetting(NodeHostDefaults.CaptureStartupErrorsKey, captureStartupErrors ? "true" : "false");
        }

        /// <summary>
        /// Specify the assembly containing the startup type to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="startupAssemblyName">The name of the assembly containing the startup type.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        [RequiresUnreferencedCode("This API searches the specified assembly for a startup type using reflection. The startup type may be trimmed. Please use UseStartup<TStartup>() to specify the startup type explicitly.")]
        public static INodeHostBuilder UseStartup(this INodeHostBuilder hostBuilder, string startupAssemblyName)
        {
            if (startupAssemblyName == null)
            {
                throw new ArgumentNullException(nameof(startupAssemblyName));
            }

            return hostBuilder
                .UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName)
                .UseSetting(NodeHostDefaults.StartupAssemblyKey, startupAssemblyName);
        }

        /// <summary>
        /// Specify the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="engine">The <see cref="IServer"/> to be used.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseEngine(this INodeHostBuilder hostBuilder, IEngine engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            return hostBuilder.ConfigureServices(services =>
            {
                // It would be nicer if this was transient but we need to pass in the
                // factory instance directly
                services.AddSingleton(engine);
            });
        }

        /// <summary>
        /// Specify the environment to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="environment">The environment to host the application in.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseEnvironment(this INodeHostBuilder hostBuilder, string environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            return hostBuilder.UseSetting(NodeHostDefaults.EnvironmentKey, environment);
        }

        /// <summary>
        /// Specify the content root directory to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="contentRoot">Path to root directory of the application.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseContentRoot(this INodeHostBuilder hostBuilder, string contentRoot)
        {
            if (contentRoot == null)
            {
                throw new ArgumentNullException(nameof(contentRoot));
            }

            return hostBuilder.UseSetting(NodeHostDefaults.ContentRootKey, contentRoot);
        }

        /// <summary>
        /// Specify the webroot directory to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="webRoot">Path to the root directory used by the web server.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseWebRoot(this INodeHostBuilder hostBuilder, string webRoot)
        {
            if (webRoot == null)
            {
                throw new ArgumentNullException(nameof(webRoot));
            }

            return hostBuilder.UseSetting(NodeHostDefaults.NodeRootKey, webRoot);
        }

        /// <summary>
        /// Specify the urls the web host will listen on.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="urls">The urls the hosted application will listen on.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseUrls(this INodeHostBuilder hostBuilder, params string[] urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException(nameof(urls));
            }

            return hostBuilder.UseSetting(NodeHostDefaults.ServerUrlsKey, string.Join(';', urls));
        }

        /// <summary>
        /// Indicate whether the host should listen on the URLs configured on the <see cref="INodeHostBuilder"/>
        /// instead of those configured on the <see cref="IServer"/>.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="preferHostingUrls"><c>true</c> to prefer URLs configured on the <see cref="INodeHostBuilder"/>; otherwise <c>false</c>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder PreferHostingUrls(this INodeHostBuilder hostBuilder, bool preferHostingUrls)
        {
            return hostBuilder.UseSetting(NodeHostDefaults.PreferHostingUrlsKey, preferHostingUrls ? "true" : "false");
        }

        /// <summary>
        /// Specify if startup status messages should be suppressed.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="suppressStatusMessages"><c>true</c> to suppress writing of hosting startup status messages; otherwise <c>false</c>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder SuppressStatusMessages(this INodeHostBuilder hostBuilder, bool suppressStatusMessages)
        {
            return hostBuilder.UseSetting(NodeHostDefaults.SuppressStatusMessagesKey, suppressStatusMessages ? "true" : "false");
        }

        /// <summary>
        /// Specify the amount of time to wait for the web host to shutdown.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="timeout">The amount of time to wait for server shutdown.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseShutdownTimeout(this INodeHostBuilder hostBuilder, TimeSpan timeout)
        {
            return hostBuilder.UseSetting(NodeHostDefaults.ShutdownTimeoutKey, ((int)timeout.TotalSeconds).ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Start the web host and listen on the specified urls.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to start.</param>
        /// <param name="urls">The urls the hosted application will listen on.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHost Start(this INodeHostBuilder hostBuilder, params string[] urls)
        {
            var host = hostBuilder.UseUrls(urls).Build();
            host.StartAsync(CancellationToken.None).GetAwaiter().GetResult();
            return host;
        }
    }

}
