using ARWNI2S.Clustering.Configuration;
using ARWNI2S.Configuration;
using ARWNI2S.Hosting;
using ARWNI2S.Hosting.Builder;
using ARWNI2S.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace ARWNI2S.Clustering
{
    /// <summary>
    /// Kestrel <see cref="INiisHostBuilder"/> extensions.
    /// </summary>
    public static class NI2SHostBuilderKestrelExtensions
    {
        /// <summary>
        /// In <see cref="UseKestrelCore(INiisHostBuilder)"/> scenarios, it may be necessary to explicitly
        /// opt in to certain HTTPS functionality.  For example, if <code>ARWNI2S_URLS</code> includes
        /// an <code>https://</code> address, <see cref="UseKestrelHttpsConfiguration"/> will enable configuration
        /// of HTTPS on that endpoint.
        ///
        /// Has no effect in <see cref="UseKestrel(INiisHostBuilder)"/> scenarios.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        public static INiisHostBuilder UseKestrelHttpsConfiguration(this INiisHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                //services.AddSingleton<HttpsConfigurationService.IInitializer, HttpsConfigurationService.Initializer>();
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
        public static INiisHostBuilder UseKestrel(this INiisHostBuilder hostBuilder)
        {
            return hostBuilder
                .UseKestrelCore()
                .UseKestrelHttpsConfiguration()
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
        /// Includes less automatic functionality than <see cref="UseKestrel(INiisHostBuilder)"/> to make trimming more effective
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
        public static INiisHostBuilder UseKestrelCore(this INiisHostBuilder hostBuilder)
        {
            //let the operating system decide what TLS protocol version to use
            //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            hostBuilder.ConfigureServices(services =>
            {
                var settings = Singleton<NI2SSettings>.Instance;

                //// Don't override an already-configured transport
                //services.TryAddSingleton<IConnectionListenerFactory, SocketTransportFactory>();

                //services.AddTransient<IConfigureOptions<KestrelServerOptions>, KestrelServerOptionsSetup>();
                //services.AddSingleton<IHttpsConfigurationService, HttpsConfigurationService>();
                services.AddSingleton<INiisRelay, ClusterNode>();
                //services.AddSingleton<KestrelMetrics>();
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
        public static INiisHostBuilder UseKestrel(this INiisHostBuilder hostBuilder, Action<KestrelServerOptions> options)
        {
            return hostBuilder.UseKestrel().ConfigureKestrel(options);
        }

        /// <summary>
        /// Configures Kestrel options but does not register an IServer. See <see cref="UseKestrel(INiisHostBuilder)"/>.
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
        public static INiisHostBuilder ConfigureKestrel(this INiisHostBuilder hostBuilder, Action<KestrelServerOptions> options)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<KestrelServerOptions>, KestrelServerOptionsSetup>());
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
        public static INiisHostBuilder UseKestrel(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, KestrelServerOptions> configureOptions)
        {
            return hostBuilder.UseKestrel().ConfigureKestrel(configureOptions);
        }

        /// <summary>
        /// Configures Kestrel options but does not register an IServer. See <see cref="UseKestrel(INiisHostBuilder)"/>.
        /// </summary>
        /// <param name="hostBuilder">
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder to configure.
        /// </param>
        /// <param name="configureOptions">A callback to configure Kestrel options.</param>
        /// <returns>
        /// The Microsoft.AspNetCore.Hosting.INiisHostBuilder.
        /// </returns>
        public static INiisHostBuilder ConfigureKestrel(this INiisHostBuilder hostBuilder, Action<NI2SHostBuilderContext, KestrelServerOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(configureOptions);

            return hostBuilder.ConfigureServices((context, services) =>
            {
                //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<KestrelServerOptions>, KestrelServerOptionsSetup>());
                services.Configure<KestrelServerOptions>(options =>
                {
                    configureOptions(context, options);
                });
            });
        }
    }
}
