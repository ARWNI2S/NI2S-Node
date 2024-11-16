using ARWNI2S.Infrastructure.Engine.Builder;
using ARWNI2S.Node.Hosting.Builder;
using ARWNI2S.Node.Hosting.Infrastructure;
using ARWNI2S.Node.Hosting.Startup;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ARWNI2S.Node.Hosting.Extensions
{
    /// <summary>
    /// Contains extensions for configuring an <see cref="INodeHostBuilder" />.
    /// </summary>
    public static class NodeHostBuilderExtensions
    {
        /// <summary>
        /// Specify the startup method to be used to configure the node enginelication.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="configureApp">The delegate that configures the <see cref="IEngineBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder Configure(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, IEngineBuilder> configureApp)
        {
            ArgumentNullException.ThrowIfNull(configureApp);

            // Light up the ISupportsStartup implementation
            if (hostBuilder is ISupportsHostStartup supportsStartup)
            {
                return supportsStartup.Configure(configureApp);
            }

            var startupAssemblyName = configureApp.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            hostBuilder.UseSetting(NodeHostDefaults.EngineKey, startupAssemblyName);

            return hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<INodeStartup>(sp =>
                {
                    return new DelegateStartup(sp.GetRequiredService<IServiceProviderFactory<IServiceCollection>>(), engine => configureApp(context, engine));
                });
            });
        }
    }
}
