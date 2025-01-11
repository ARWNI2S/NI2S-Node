// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Cluster.Configuration;
using ARWNI2S.Cluster.Diagnostics;
using ARWNI2S.Cluster.Lifecycle;
using ARWNI2S.Cluster.Networking.Connection;
using ARWNI2S.Hosting;
using ARWNI2S.Hosting.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Orleans;

namespace ARWNI2S.Cluster.Hosting
{
    /// <summary>
    /// Kestrel <see cref="INiisHostBuilder"/> extensions.
    /// </summary>
    internal static class ClusterNodeHostBuilderExtensions
    {
        /// <summary>
        /// In <see cref="UseClusterCore(INiisHostBuilder)"/> scenarios, it may be necessary to explicitly
        /// opt in to certain HTTPS functionality.  For example, if <code>ARWNI2S_URLS</code> includes
        /// an <code>https://</code> address, <see cref="UseClusterNI2SConfiguration"/> will enable configuration
        /// of HTTPS on that endpoint.
        ///
        /// Has no effect in <see cref="UseClustering(INiisHostBuilder)"/> scenarios.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        internal static INiisHostBuilder UseClusterNI2SConfiguration(this INiisHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>//3.1
            {
                //services.AddSingleton<NI2SConfigurationService.IInitializer, NI2SConfigurationService.Initializer>();
            });
        }

        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        internal static INiisHostBuilder UseClustering(this INiisHostBuilder hostBuilder)
        {
            return hostBuilder
                .UseClusterCore()
                .UseClusterNI2SConfiguration()
                //.UseQuic(options =>
                //{
                //    // Configure server defaults to match client defaults.
                //    // https://github.com/dotnet/runtime/blob/a5f3676cc71e176084f0f7f1f6beeecd86fbeafc/src/libraries/System.Net.Http/src/System/Net/Http/SocketsHttpHandler/ConnectHelper.cs#L118-L119
                //    options.DefaultStreamErrorCode = (long)Http3ErrorCode.RequestCancelled;
                //    options.DefaultCloseErrorCode = (long)Http3ErrorCode.NoError;
                //})
                ;
        }

        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// Includes less automatic functionality than <see cref="UseClustering(INiisHostBuilder)"/> to make trimming more effective
        /// (e.g. for <see href="https://aka.ms/aspnet/nativeaot">Native AOT</see> scenarios).  If the host ends up depending on
        /// some of the absent functionality, a best-effort attempt will be made to enable it on-demand.  Failing that, an
        /// exception with an informative error message will be raised when the host is started.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        internal static INiisHostBuilder UseClusterCore(this INiisHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>//3
            {
                // Don't override an already-configured transport
                services.TryAddSingleton<IConnectionListenerFactory, ClusterTransportFactory>();

                services.AddTransient<IConfigureOptions<ClusterNodeOptions>, ClusterNodeOptionsSetup>();

                services.AddSingleton<ClusterNodeLifecycle>();
                services.AddSingleton<IClusterNodeLifecycle>(provider => provider.GetRequiredService<ClusterNodeLifecycle>());
                services.AddSingleton<ILifecycleSubject>(provider => provider.GetRequiredService<ClusterNodeLifecycle>());

                services.AddSingleton<IClusterConfigurationService, ClusterConfigurationService>();
                services.AddSingleton<IClusterNode, NI2SNodeLocal>();
                services.AddSingleton<ClusterNodeMetrics>();
            });

            if (OperatingSystem.IsWindows())
            {
                //hostBuilder.UseNamedPipes();
            }

            return hostBuilder;
        }

        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <param name="options">
        /// A callback to configure Kestrel options.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        internal static INiisHostBuilder UseClustering(this INiisHostBuilder hostBuilder, Action<ClusterNodeOptions> options)
        {
            return hostBuilder.UseClustering().ConfigureCluster(options);
        }

        /// <summary>
        /// Configures Kestrel options but does not register an IServer. See <see cref="UseClustering(INiisHostBuilder)"/>.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <param name="options">
        /// A callback to configure Kestrel options.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        internal static INiisHostBuilder ConfigureCluster(this INiisHostBuilder hostBuilder, Action<ClusterNodeOptions> options)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ClusterNodeOptions>, ClusterNodeOptionsSetup>());
                services.Configure(options);
            });
        }

        /// <summary>
        /// Specify Kestrel as the server to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <param name="configureOptions">A callback to configure Kestrel options.</param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        internal static INiisHostBuilder UseClustering(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, ClusterNodeOptions> configureOptions)
        {
            return hostBuilder.UseClustering().ConfigureCluster(configureOptions);
        }

        /// <summary>
        /// Configures Kestrel options but does not register an IServer. See <see cref="UseClustering(INiisHostBuilder)"/>.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <param name="configureOptions">A callback to configure Kestrel options.</param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        internal static INiisHostBuilder ConfigureCluster(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, ClusterNodeOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(configureOptions);

            return hostBuilder.ConfigureServices((context, services) =>//3.2
            {
                //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ClusterNodeOptions>, ClusterNodeOptionsSetup>());
                services.Configure<ClusterNodeOptions>(options =>
                {
                    configureOptions(context, options);
                });
            });
        }
    }
}
