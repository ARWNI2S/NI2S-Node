// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NI2S.Node.Core;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using System;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// NodeEngine <see cref="INodeHostBuilder"/> extensions.
    /// </summary>
    public static class NodeHostBuilderNodeEngineExtensions
    {
        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder to configure.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder.
        /// </returns>
        /* 001.2.1.3.1.2.1 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...) -> builder.ConfigureNodeHost(...)
                             -> configure(nodehostBuilder) -> NodeEngine.ConfigureNodeDefaults(nodehostBuilder) -> builder.UseNodeEngine(...) */
        public static INodeHostBuilder UseNodeEngine(this INodeHostBuilder hostBuilder)
        {
            hostBuilder.UseEngineModules(options =>
            {
                //options.DefaultStreamErrorCode = (long)Http3ErrorCode.RequestCancelled;
                //options.DefaultCloseErrorCode = (long)Http3ErrorCode.NoError;
            });

            return hostBuilder.ConfigureServices(services =>
            {
                /* 001.3.6.2.1 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.RunDefaultCallbacks() -> configureServicesAction(Context, _builder.Configuration)
                                 -> (context, services) => configureServices(services) */
                // Don't override an already-configured event handler
                services.TryAddSingleton<IEventHandlerAccessor, DefaultEventHandlerAccessor>();

                services.AddTransient<IConfigureOptions<NodeEngineOptions>, NodeEngineOptionsSetup>();
                services.AddSingleton<INodeEngine, NodeEngineImpl>();
            });
        }

        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder to configure.
        /// </param>
        /// <param name="options">
        /// A callback to configure Kestrel options.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder.
        /// </returns>
        public static INodeHostBuilder UseNodeEngine(this INodeHostBuilder hostBuilder, Action<NodeEngineOptions> options)
        {
            return hostBuilder.UseNodeEngine().ConfigureNodeEngine(options);
        }

        /// <summary>
        /// Configures Kestrel options but does not register an IServer. See <see cref="UseNodeEngine(INodeHostBuilder)"/>.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder to configure.
        /// </param>
        /// <param name="options">
        /// A callback to configure Kestrel options.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder.
        /// </returns>
        public static INodeHostBuilder ConfigureNodeEngine(this INodeHostBuilder hostBuilder, Action<NodeEngineOptions> options)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<NodeEngineOptions>, NodeEngineOptionsSetup>());
                services.Configure(options);
            });
        }

        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder to configure.
        /// </param>
        /// <param name="configureOptions">A callback to configure Kestrel options.</param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder.
        /// </returns>
        /* 001.2.1.3.1.2 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...) -> builder.ConfigureNodeHost(...)
                          -> configure(nodehostBuilder) -> NodeEngine.ConfigureNodeDefaults(nodehostBuilder) -> builder.UseNodeEngine(...) */
        public static INodeHostBuilder UseNodeEngine(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, NodeEngineOptions> configureOptions)
        {
            return hostBuilder.UseNodeEngine().ConfigureNodeEngine(configureOptions);
        }

        /// <summary>
        /// Configures Kestrel options but does not register an IServer. See <see cref="UseNodeEngine(INodeHostBuilder)"/>.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder to configure.
        /// </param>
        /// <param name="configureOptions">A callback to configure Kestrel options.</param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder.
        /// </returns>
        /* 001.2.1.3.1.2.2 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...) -> builder.ConfigureNodeHost(...)
                             -> configure(nodehostBuilder) -> NodeEngine.ConfigureNodeDefaults(nodehostBuilder) -> builder.UseNodeEngine(...).ConfigureNodeEngine(...) */
        public static INodeHostBuilder ConfigureNodeEngine(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, NodeEngineOptions> configureOptions)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            return hostBuilder.ConfigureServices((context, services) =>
            {
                /* 001.3.7.2.1 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.RunDefaultCallbacks() -> configureServicesAction(Context, _builder.Configuration)
                                 -> (context, services) => configureServices(services) */
                services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<NodeEngineOptions>, NodeEngineOptionsSetup>());
                services.Configure<NodeEngineOptions>(options =>
                {
                    configureOptions(context, options);
                });
            });
        }

        /* 001.2.1.3.1.2.1.1.1 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...) -> builder.ConfigureNodeHost(...)
                                 -> configure(nodehostBuilder) -> NodeEngine.ConfigureNodeDefaults(nodehostBuilder) -> builder.UseNodeEngine(...)
                                 -> builder.UseEngineModules(...) */
        public static INodeHostBuilder UseEngineModules(this INodeHostBuilder hostBuilder)
        {
            return hostBuilder;
        }

        /* 001.2.1.3.1.2.1.1.2 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...) -> builder.ConfigureNodeHost(...)
                              -> configure(nodehostBuilder) -> NodeEngine.ConfigureNodeDefaults(nodehostBuilder) -> builder.UseNodeEngine(...)
                              -> builder.UseEngineModules(...).ConfigureEngineModules(options) */
        public static INodeHostBuilder ConfigureEngineModules(this INodeHostBuilder hostBuilder, Action<EngineModulesOptions> options)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                /* 001.3.5.2.1 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.RunDefaultCallbacks() -> configureServicesAction(Context, _builder.Configuration)
                                 -> (context, services) => configureServices(services) */
                services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<EngineModulesOptions>, EngineModulesOptionsSetup>());
                services.Configure(options);
            });
        }

        /* 001.2.1.3.1.2.1.1 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...) -> builder.ConfigureNodeHost(...)
                               -> configure(nodehostBuilder) -> NodeEngine.ConfigureNodeDefaults(nodehostBuilder) -> builder.UseNodeEngine(...)
                               -> builder.UseEngineModules(...) */
        public static INodeHostBuilder UseEngineModules(this INodeHostBuilder hostBuilder, Action<EngineModulesOptions> options)
        {
            return hostBuilder.UseEngineModules().ConfigureEngineModules(options);
        }
    }
}
