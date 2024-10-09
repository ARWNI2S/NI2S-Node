using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Extensions
{
    public static class HostBuilderExtensions
    {
        public static INodeHostBuilder AsNodeHostBuilder(this IHostBuilder hostBuilder)
        {
            if (hostBuilder is INodeHostBuilder nodeHostBuilder)
            {
                return nodeHostBuilder;
            }

            return new NodeHostBuilder(hostBuilder);
        }

        //public static INodeHostBuilder UsePipelineFilterFactory(this INodeHostBuilder hostBuilder, Func<object, IPipelineFilter> filterFactory)
        //{
        //    hostBuilder.ConfigureServices(
        //        (hostCtx, services) =>
        //        {
        //            services.AddSingleton<Func<object, IPipelineFilter>>(filterFactory);
        //        }
        //    );

        //    return hostBuilder.UsePipelineFilterFactory<DelegatePipelineFilterFactory>();
        //}

        //public static INodeHostBuilder UseClearIdleSession(this INodeHostBuilder hostBuilder)
        //{
        //    return hostBuilder.UseMiddleware<ClearIdleSessionMiddleware>();
        //}

        //public static INodeHostBuilder UseSessionHandler(this INodeHostBuilder hostBuilder, Func<IAppSession, ValueTask> onConnected = null, Func<IAppSession, CloseEventArgs, ValueTask> onClosed = null)
        //{
        //    return hostBuilder.ConfigureServices(
        //        (hostCtx, services) =>
        //        {
        //            services.AddSingleton<SessionHandlers>(new SessionHandlers
        //            {
        //                Connected = onConnected,
        //                Closed = onClosed
        //            });
        //        }
        //    );
        //}

        //public static INodeHostBuilder ConfigureSuperSocket(this INodeHostBuilder hostBuilder, Action<ServerOptions> configurator)
        //{
        //    return hostBuilder.ConfigureServices(
        //        (hostCtx, services) =>
        //        {
        //            services.Configure<ServerOptions>(configurator);
        //        }
        //    );
        //}

        //public static INodeHostBuilder ConfigureSocketOptions(this INodeHostBuilder hostBuilder, Action<Socket> socketOptionsSetter)
        //    where TReceivePackage : class
        //{
        //    return hostBuilder.ConfigureServices(
        //        (hostCtx, services) =>
        //        {
        //            services.AddSingleton<SocketOptionsSetter>(new SocketOptionsSetter(socketOptionsSetter));
        //        }
        //    );
        //}

        //public static IServer BuildAsServer(this IHostBuilder hostBuilder)
        //{
        //    var host = hostBuilder.Build();
        //    return host.AsServer();
        //}

        //public static IServer AsServer(this IHost host)
        //{
        //    return host.Services.GetService<IEnumerable<IHostedService>>().OfType<IServer>().FirstOrDefault();
        //}

        //public static INodeHostBuilder ConfigureErrorHandler(this INodeHostBuilder hostBuilder, Func<IAppSession, PackageHandlingException, ValueTask<bool>> errorHandler)
        //{
        //    return hostBuilder.ConfigureServices(
        //        (hostCtx, services) =>
        //        {
        //            services.AddSingleton<Func<IAppSession, PackageHandlingException, ValueTask<bool>>>(errorHandler);
        //        }
        //    );
        //}

        //// move to extensions
        //public static INodeHostBuilder UsePackageHandler(this INodeHostBuilder hostBuilder, Func<IAppSession, TReceivePackage, ValueTask> packageHandler, Func<IAppSession, PackageHandlingException, ValueTask<bool>> errorHandler = null)
        //{
        //    return hostBuilder.ConfigureServices(
        //        (hostCtx, services) =>
        //        {
        //            if (packageHandler != null)
        //                services.AddSingleton<IPackageHandler>(new DelegatePackageHandler(packageHandler));

        //            if (errorHandler != null)
        //                services.AddSingleton<Func<IAppSession, PackageHandlingException, ValueTask<bool>>>(errorHandler);
        //        }
        //    );
        //}

        //public static MultipleServerHostBuilder AsMultipleServerHostBuilder(this IHostBuilder hostBuilder)
        //{
        //    return new MultipleServerHostBuilder(hostBuilder);
        //}

        public static IMinimalApiHostBuilder AsMinimalApiHostBuilder(this INodeHostBuilder hostBuilder)
        {
            return hostBuilder;
        }

        //public static INodeHostBuilder UseGZip(this INodeHostBuilder hostBuilder)
        //{
        //    return hostBuilder.ConfigureServices((hostCtx, services) =>
        //    {
        //        services.AddSingleton<IConnectionStreamInitializersFactory>(new DefaultConnectionStreamInitializersFactory(CompressionLevel.Optimal));
        //    }) as INodeHostBuilder;
        //}
    }
}
