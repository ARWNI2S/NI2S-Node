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
    /// Kestrel <see cref="INodeHostBuilder"/> extensions.
    /// </summary>
    public static class NodeHostBuilderNI2SExtensions
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
        public static INodeHostBuilder UseNI2S(this INodeHostBuilder hostBuilder)
        {
            //hostBuilder.UseGdesk(options =>
            //{
            //    options.DefaultStreamErrorCode = (long)Http3ErrorCode.RequestCancelled;
            //    options.DefaultCloseErrorCode = (long)Http3ErrorCode.NoError;
            //});

            return hostBuilder.ConfigureServices(services =>
            {
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
        public static INodeHostBuilder UseNI2S(this INodeHostBuilder hostBuilder, Action<NodeEngineOptions> options)
        {
            return hostBuilder.UseNI2S().ConfigureNI2S(options);
        }

        /// <summary>
        /// Configures Kestrel options but does not register an IServer. See <see cref="UseNI2S(INodeHostBuilder)"/>.
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
        public static INodeHostBuilder ConfigureNI2S(this INodeHostBuilder hostBuilder, Action<NodeEngineOptions> options)
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
        public static INodeHostBuilder UseNI2S(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, NodeEngineOptions> configureOptions)
        {
            return hostBuilder.UseNI2S().ConfigureNI2S(configureOptions);
        }

        /// <summary>
        /// Configures Kestrel options but does not register an IServer. See <see cref="UseNI2S(INodeHostBuilder)"/>.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder to configure.
        /// </param>
        /// <param name="configureOptions">A callback to configure Kestrel options.</param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INodeHostBuilder.
        /// </returns>
        public static INodeHostBuilder ConfigureNI2S(this INodeHostBuilder hostBuilder, Action<NodeHostBuilderContext, NodeEngineOptions> configureOptions)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            return hostBuilder.ConfigureServices((context, services) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<NodeEngineOptions>, NodeEngineOptionsSetup>());
                services.Configure<NodeEngineOptions>(options =>
                {
                    configureOptions(context, options);
                });
            });
        }
    }
}
