using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Connection;
using ARWNI2S.Engine.Network.Connections;
using ARWNI2S.Engine.Network.Host;
using ARWNI2S.Engine.Network.Protocol.Factory;
using ARWNI2S.Engine.Network.Session;
using ARWNI2S.Infrastructure.Network.Connection;
using ARWNI2S.Infrastructure.Network.Protocol;
using ARWNI2S.Runtime.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using System.Net.Sockets;

namespace ARWNI2S.Runtime.Hosting.Extensions
{
    public static class HostBuilderExtensions
    {
        public static INodeServerHostBuilder<TReceivePackage> AsNodeServerHostBuilder<TReceivePackage>(this IHostBuilder hostBuilder)
        {
            if (hostBuilder is INodeServerHostBuilder<TReceivePackage> ssHostBuilder)
            {
                return ssHostBuilder;
            }

            return new NodeServerHostBuilder<TReceivePackage>(hostBuilder);
        }

        public static INodeServerHostBuilder<TReceivePackage> AsNodeServerHostBuilder<TReceivePackage, TPipelineFilter>(this IHostBuilder hostBuilder)
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            if (hostBuilder is INodeServerHostBuilder<TReceivePackage> ssHostBuilder)
            {
                return ssHostBuilder;
            }

            return new NodeServerHostBuilder<TReceivePackage>(hostBuilder)
                .UsePipelineFilter<TPipelineFilter>();
        }

        public static INodeServerHostBuilder<TReceivePackage> UsePipelineFilterFactory<TReceivePackage>(this INodeServerHostBuilder<TReceivePackage> hostBuilder, Func<object, IPipelineFilter<TReceivePackage>> filterFactory)
        {
            hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton(filterFactory);
                }
            );

            return hostBuilder.UsePipelineFilterFactory<DelegatePipelineFilterFactory<TReceivePackage>>();
        }

        public static INodeServerHostBuilder<TReceivePackage> UseClearIdleSession<TReceivePackage>(this INodeServerHostBuilder<TReceivePackage> hostBuilder)
        {
            return hostBuilder.UseMiddleware<ClearIdleSessionMiddleware>();
        }

        public static INodeServerHostBuilder<TReceivePackage> UseSessionHandler<TReceivePackage>(this INodeServerHostBuilder<TReceivePackage> hostBuilder, Func<INodeSession, ValueTask> onConnected = null, Func<INodeSession, CloseEventArgs, ValueTask> onClosed = null)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton(new SessionHandlers
                    {
                        Connected = onConnected,
                        Closed = onClosed
                    });
                }
            );
        }

        public static INodeServerHostBuilder<TReceivePackage> ConfigureNodeServer<TReceivePackage>(this INodeServerHostBuilder<TReceivePackage> hostBuilder, Action<ServerOptions> configurator)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.Configure(configurator);
                }
            );
        }

        public static INodeServerHostBuilder<TReceivePackage> ConfigureSocketOptions<TReceivePackage>(this INodeServerHostBuilder<TReceivePackage> hostBuilder, Action<Socket> socketOptionsSetter)
            where TReceivePackage : class
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton(new SocketOptionsSetter(socketOptionsSetter));
                }
            );
        }

        public static IServer BuildAsServer(this IHostBuilder hostBuilder)
        {
            var host = hostBuilder.Build();
            return host.AsServer();
        }

        public static IServer AsServer(this IHost host)
        {
            return host.Services.GetService<IEnumerable<IHostedService>>().OfType<IServer>().FirstOrDefault();
        }

        public static INodeServerHostBuilder<TReceivePackage> ConfigureErrorHandler<TReceivePackage>(this INodeServerHostBuilder<TReceivePackage> hostBuilder, Func<INodeSession, PackageHandlingException<TReceivePackage>, ValueTask<bool>> errorHandler)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton(errorHandler);
                }
            );
        }

        // move to extensions
        public static INodeServerHostBuilder<TReceivePackage> UsePackageHandler<TReceivePackage>(this INodeServerHostBuilder<TReceivePackage> hostBuilder, Func<INodeSession, TReceivePackage, ValueTask> packageHandler, Func<INodeSession, PackageHandlingException<TReceivePackage>, ValueTask<bool>> errorHandler = null)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    if (packageHandler != null)
                        services.AddSingleton<IPackageHandler<TReceivePackage>>(new DelegatePackageHandler<TReceivePackage>(packageHandler));

                    if (errorHandler != null)
                        services.AddSingleton(errorHandler);
                }
            );
        }

        public static MultipleServerHostBuilder AsMultipleServerHostBuilder(this IHostBuilder hostBuilder)
        {
            return new MultipleServerHostBuilder(hostBuilder);
        }

        public static IMinimalApiHostBuilder AsMinimalApiHostBuilder(this INodeServerHostBuilder hostBuilder)
        {
            return hostBuilder;
        }

        public static INodeServerHostBuilder UseGZip(this INodeServerHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostCtx, services) =>
            {
                services.AddSingleton<IConnectionStreamInitializersFactory>(new DefaultConnectionStreamInitializersFactory(CompressionLevel.Optimal));
            }) as INodeServerHostBuilder;
        }
    }
}
