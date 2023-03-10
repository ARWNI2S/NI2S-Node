using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Session;
using System.Net.Sockets;

namespace NI2S.Node.Hosting
{
    public static class HostBuilderExtensions
    {
        public static INodeHostBuilderOld<TReceivePackage> AsNodeHostBuilder<TReceivePackage>(this IHostBuilder hostBuilder)
        {
            if (hostBuilder is INodeHostBuilderOld<TReceivePackage> ssHostBuilder)
            {
                return ssHostBuilder;
            }

            return new NodeHostBuilder<TReceivePackage>(hostBuilder);
        }

        public static INodeHostBuilderOld<TReceivePackage> AsNodeHostBuilder<TReceivePackage, TPipelineFilter>(this IHostBuilder hostBuilder)
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            if (hostBuilder is INodeHostBuilderOld<TReceivePackage> ssHostBuilder)
            {
                return ssHostBuilder;
            }

            return new NodeHostBuilder<TReceivePackage>(hostBuilder)
                .UsePipelineFilter<TPipelineFilter>();
        }

        public static INodeHostBuilderOld<TReceivePackage> UsePipelineFilterFactory<TReceivePackage>(this INodeHostBuilderOld<TReceivePackage> hostBuilder, Func<object, IPipelineFilter<TReceivePackage>> filterFactory)
        {
            hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton(filterFactory);
                }
            );

            return hostBuilder.UsePipelineFilterFactory<DelegatePipelineFilterFactory<TReceivePackage>>();
        }

        public static INodeHostBuilderOld<TReceivePackage> UseClearIdleSession<TReceivePackage>(this INodeHostBuilderOld<TReceivePackage> hostBuilder)
        {
            return hostBuilder.UseMiddleware<ClearIdleSessionMiddleware>();
        }

        public static INodeHostBuilderOld<TReceivePackage> UseSessionHandler<TReceivePackage>(this INodeHostBuilderOld<TReceivePackage> hostBuilder, Func<ISession, ValueTask>? onConnected = null, Func<ISession, CloseEventArgs, ValueTask>? onClosed = null)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton(new NodeService<TReceivePackage>.SessionHandlers
                    {
                        Connected = onConnected,
                        Closed = onClosed
                    });
                }
            );
        }

        public static INodeHostBuilderOld<TReceivePackage> ConfigureNode<TReceivePackage>(this INodeHostBuilderOld<TReceivePackage> hostBuilder, Action<ServerOptions> configurator)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.Configure(configurator);
                }
            );
        }

        public static INodeHostBuilderOld<TReceivePackage> ConfigureSocketOptions<TReceivePackage>(this INodeHostBuilderOld<TReceivePackage> hostBuilder, Action<Socket> socketOptionsSetter)
            where TReceivePackage : class
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton(new SocketOptionsSetter(socketOptionsSetter));
                }
            );
        }

        public static INode? BuildAsServer(this IHostBuilder hostBuilder)
        {
            var host = hostBuilder.Build();
            return host.AsServer();
        }

        public static INode? AsServer(this IHost host)
        {
            return host.Services.GetService<IEnumerable<IHostedService>>()?.OfType<INode>().FirstOrDefault();
        }

        public static INodeHostBuilderOld<TReceivePackage> ConfigureErrorHandler<TReceivePackage>(this INodeHostBuilderOld<TReceivePackage> hostBuilder, Func<ISession, PackageHandlingException<TReceivePackage>, ValueTask<bool>> errorHandler)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton(errorHandler);
                }
            );
        }

        // move to extensions
        public static INodeHostBuilderOld<TReceivePackage> UsePackageHandler<TReceivePackage>(this INodeHostBuilderOld<TReceivePackage> hostBuilder, Func<ISession, TReceivePackage, ValueTask> packageHandler, Func<ISession, PackageHandlingException<TReceivePackage>, ValueTask<bool>>? errorHandler = null)
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

        public static IMinimalApiHostBuilderOld AsMinimalApiHostBuilder(this INodeHostBuilderOld hostBuilder)
        {
            return hostBuilder;
        }
    }
}
