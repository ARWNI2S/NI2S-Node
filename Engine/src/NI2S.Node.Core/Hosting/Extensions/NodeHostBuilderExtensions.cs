// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using NI2S.Node.Hosting.Infrastructure;
using NI2S.Node.Hosting.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Contains extensions for configuring an <see cref="INodeHostBuilder" />.
    /// </summary>
    public static class NodeHostBuilderExtensions
    {
        /// <summary>
        /// Specify the startup method to be used to configure the web application.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="configureApp">The delegate that configures the <see cref="INodeEngineBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder Configure(this INodeHostBuilder hostBuilder, Action<INodeEngineBuilder> configureApp)
        {
            if (configureApp == null)
            {
                throw new ArgumentNullException(nameof(configureApp));
            }

            // Light up the ISupportsStartup implementation
            if (hostBuilder is ISupportsStartup supportsStartup)
            {
                return supportsStartup.Configure(configureApp);
            }

            var startupAssemblyName = configureApp.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            hostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName);

            return hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IStartup>(sp =>
                {
                    return new NodeEngineStartup(sp.GetRequiredService<IServiceProviderFactory<IServiceCollection>>(), (app => configureApp(app)));
                });
            });
        }

        /// <summary>
        /// Specify the startup method to be used to configure the web application.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="configureApp">The delegate that configures the <see cref="INodeEngineBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        /* 001.2.1.3.2.1 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...) -> builder.ConfigureNodeHost(...)
                        -> configure(nodehostBuilder) -> configure(nodeHostBuilder) -> nodeHostBuilder.Configure(...) */
        public static INodeHostBuilder Configure(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, INodeEngineBuilder> configureApp)
        {
            if (configureApp == null)
            {
                throw new ArgumentNullException(nameof(configureApp));
            }

            // Light up the ISupportsStartup implementation
            if (hostBuilder is ISupportsStartup supportsStartup)
            {
                return supportsStartup.Configure(configureApp);
            }

            var startupAssemblyName = configureApp.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            hostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName);

            return hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IStartup>(sp =>
                {
                    return new NodeEngineStartup(sp.GetRequiredService<IServiceProviderFactory<IServiceCollection>>(), (app => configureApp(context, app)));
                });
            });
        }

        /// <summary>
        /// Specify a factory that creates the startup instance to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="startupFactory">A delegate that specifies a factory for the startup class.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        /// <remarks>When using the il linker, all public methods of <typeparamref name="TStartup"/> are preserved. This should match the Startup type directly (and not a base type).</remarks>
        public static INodeHostBuilder UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(this INodeHostBuilder hostBuilder, Func<NodeHostBuilderContext, TStartup> startupFactory) where TStartup : class
        {
            if (startupFactory == null)
            {
                throw new ArgumentNullException(nameof(startupFactory));
            }

            // Light up the GenericNodeHostBuilder implementation
            if (hostBuilder is ISupportsStartup supportsStartup)
            {
                return supportsStartup.UseStartup(startupFactory);
            }

            var startupAssemblyName = startupFactory.GetMethodInfo().DeclaringType!.Assembly.GetName().Name;

            hostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName);

            return hostBuilder
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(typeof(IStartup), GetStartupInstance);

                    [UnconditionalSuppressMessage("Trimmer", "IL2072", Justification = "Startup type created by factory can't be determined statically.")]
                    object GetStartupInstance(IServiceProvider serviceProvider)
                    {
                        var instance = startupFactory(context) ?? throw new InvalidOperationException("The specified factory returned null startup instance.");

                        var hostingEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();

                        // Check if the instance implements IStartup before wrapping
                        if (instance is IStartup startup)
                        {
                            return startup;
                        }

                        return new ConventionBasedStartup(StartupLoader.LoadMethods(serviceProvider, instance.GetType(), hostingEnvironment.EnvironmentName, instance));
                    }
                });
        }

        /// <summary>
        /// Specify the startup type to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="startupType">The <see cref="Type"/> to be used.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseStartup(this INodeHostBuilder hostBuilder, [DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType)
        {
            if (startupType == null)
            {
                throw new ArgumentNullException(nameof(startupType));
            }

            // Light up the GenericNodeHostBuilder implementation
            if (hostBuilder is ISupportsStartup supportsStartup)
            {
                return supportsStartup.UseStartup(startupType);
            }

            var startupAssemblyName = startupType.Assembly.GetName().Name;

            hostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, startupAssemblyName);

            var state = new UseStartupState(startupType);

            return hostBuilder
                .ConfigureServices(services =>
                {
                    if (typeof(IStartup).IsAssignableFrom(state.StartupType))
                    {
                        services.AddSingleton(typeof(IStartup), state.StartupType);
                    }
                    else
                    {
                        services.AddSingleton(typeof(IStartup), sp =>
                        {
                            var hostingEnvironment = sp.GetRequiredService<IHostEnvironment>();
                            return new ConventionBasedStartup(StartupLoader.LoadMethods(sp, state.StartupType, hostingEnvironment.EnvironmentName));
                        });
                    }
                });
        }

        /// <summary>
        /// Specify the startup type to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <typeparam name ="TStartup">The type containing the startup methods for the application.</typeparam>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseStartup<[DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] TStartup>(this INodeHostBuilder hostBuilder) where TStartup : class
        {
            return hostBuilder.UseStartup(typeof(TStartup));
        }

        /// <summary>
        /// Configures the default service provider
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="configure">A callback used to configure the <see cref="ServiceProviderOptions"/> for the default <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseDefaultServiceProvider(this INodeHostBuilder hostBuilder, Action<ServiceProviderOptions> configure)
        {
            return hostBuilder.UseDefaultServiceProvider((context, options) => configure(options));
        }

        /// <summary>
        /// Configures the default service provider
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="configure">A callback used to configure the <see cref="ServiceProviderOptions"/> for the default <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseDefaultServiceProvider(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, ServiceProviderOptions> configure)
        {
            // Light up the GenericNodeHostBuilder implementation
            if (hostBuilder is ISupportsUseDefaultServiceProvider supportsDefaultServiceProvider)
            {
                return supportsDefaultServiceProvider.UseDefaultServiceProvider(configure);
            }

            return hostBuilder.ConfigureServices((context, services) =>
            {
                var options = new ServiceProviderOptions();
                configure(context, options);
                //services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>>(new DefaultServiceProviderFactory(options)));
            });
        }

        /// <summary>
        /// Adds a delegate for configuring the <see cref="IConfigurationBuilder"/> that will construct an <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder"/> to configure.</param>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder" /> that will be used to construct an <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        /// <remarks>
        /// The <see cref="IConfiguration"/> and <see cref="ILoggerFactory"/> on the <see cref="NodeHostBuilderContext"/> are uninitialized at this stage.
        /// The <see cref="IConfigurationBuilder"/> is pre-populated with the settings of the <see cref="INodeHostBuilder"/>.
        /// </remarks>
        public static INodeHostBuilder ConfigureAppConfiguration(this INodeHostBuilder hostBuilder, Action<IConfigurationBuilder> configureDelegate)
        {
            return hostBuilder.ConfigureNodeConfiguration((context, builder) => configureDelegate(builder));
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder ConfigureLogging(this INodeHostBuilder hostBuilder, Action<ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices(collection => collection.AddLogging(configureLogging));
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="LoggerFactory"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="INodeHostBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="LoggerFactory"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder ConfigureLogging(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(context, builder)));
        }

        /// <summary>
        /// Configures the <see cref="INodeHostEnvironment.NodeRootFileProvider"/> to use static web assets
        /// defined by referenced projects and packages.
        /// </summary>
        /// <param name="builder">The <see cref="INodeHostBuilder"/>.</param>
        /// <returns>The <see cref="INodeHostBuilder"/>.</returns>
        public static INodeHostBuilder UseStaticNodeAssets(this INodeHostBuilder builder)
        {
            builder.ConfigureNodeConfiguration((context, configBuilder) =>
            {
                //StaticNodeAssetsLoader.UseStaticNodeAssets(context.HostingEnvironment, context.Configuration);
            });

            return builder;
        }
    }
}
