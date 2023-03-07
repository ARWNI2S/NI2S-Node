using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Session;
using NI2S.Node.Server;
using System.Net.Sockets;

namespace NI2S.Node
{
    public static class HostBuilderExtensions
    {
        public static INodeHostBuilder<TReceivePackage> AsSuperSocketHostBuilder<TReceivePackage>(this IHostBuilder hostBuilder)
        {
            if (hostBuilder is INodeHostBuilder<TReceivePackage> ssHostBuilder)
            {
                return ssHostBuilder;
            }

            return new SuperSocketHostBuilder<TReceivePackage>(hostBuilder);
        }

        public static INodeHostBuilder<TReceivePackage> AsSuperSocketHostBuilder<TReceivePackage, TPipelineFilter>(this IHostBuilder hostBuilder)
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            if (hostBuilder is INodeHostBuilder<TReceivePackage> ssHostBuilder)
            {
                return ssHostBuilder;
            }

            return (new SuperSocketHostBuilder<TReceivePackage>(hostBuilder))
                .UsePipelineFilter<TPipelineFilter>();
        }

        public static INodeHostBuilder<TReceivePackage> UsePipelineFilterFactory<TReceivePackage>(this INodeHostBuilder<TReceivePackage> hostBuilder, Func<object, IPipelineFilter<TReceivePackage>> filterFactory)
        {
            hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<Func<object, IPipelineFilter<TReceivePackage>>>(filterFactory);
                }
            );

            return hostBuilder.UsePipelineFilterFactory<DelegatePipelineFilterFactory<TReceivePackage>>();
        }

        public static INodeHostBuilder<TReceivePackage> UseClearIdleSession<TReceivePackage>(this INodeHostBuilder<TReceivePackage> hostBuilder)
        {
            return hostBuilder.UseMiddleware<ClearIdleSessionMiddleware>();
        }

        public static INodeHostBuilder<TReceivePackage> UseSessionHandler<TReceivePackage>(this INodeHostBuilder<TReceivePackage> hostBuilder, Func<ISession, ValueTask>? onConnected = null, Func<ISession, CloseEventArgs, ValueTask>? onClosed = null)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<SessionHandlers>(new SessionHandlers
                    {
                        Connected = onConnected,
                        Closed = onClosed
                    });
                }
            );
        }

        public static INodeHostBuilder<TReceivePackage> ConfigureSuperSocket<TReceivePackage>(this INodeHostBuilder<TReceivePackage> hostBuilder, Action<ServerOptions> configurator)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.Configure<ServerOptions>(configurator);
                }
            );
        }

        public static INodeHostBuilder<TReceivePackage> ConfigureSocketOptions<TReceivePackage>(this INodeHostBuilder<TReceivePackage> hostBuilder, Action<Socket> socketOptionsSetter)
            where TReceivePackage : class
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<SocketOptionsSetter>(new SocketOptionsSetter(socketOptionsSetter));
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

        public static INodeHostBuilder<TReceivePackage> ConfigureErrorHandler<TReceivePackage>(this INodeHostBuilder<TReceivePackage> hostBuilder, Func<ISession, PackageHandlingException<TReceivePackage>, ValueTask<bool>> errorHandler)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    services.AddSingleton<Func<ISession, PackageHandlingException<TReceivePackage>, ValueTask<bool>>>(errorHandler);
                }
            );
        }

        // move to extensions
        public static INodeHostBuilder<TReceivePackage> UsePackageHandler<TReceivePackage>(this INodeHostBuilder<TReceivePackage> hostBuilder, Func<ISession, TReceivePackage, ValueTask> packageHandler, Func<ISession, PackageHandlingException<TReceivePackage>, ValueTask<bool>>? errorHandler = null)
        {
            return hostBuilder.ConfigureServices(
                (hostCtx, services) =>
                {
                    if (packageHandler != null)
                        services.AddSingleton<IPackageHandler<TReceivePackage>>(new DelegatePackageHandler<TReceivePackage>(packageHandler));

                    if (errorHandler != null)
                        services.AddSingleton<Func<ISession, PackageHandlingException<TReceivePackage>, ValueTask<bool>>>(errorHandler);
                }
            );
        }

        public static MultipleServerHostBuilder AsMultipleServerHostBuilder(this IHostBuilder hostBuilder)
        {
            return new MultipleServerHostBuilder(hostBuilder);
        }

        public static IMinimalApiHostBuilder AsMinimalApiHostBuilder(this INodeHostBuilder hostBuilder)
        {
            return hostBuilder;
        }
    }
}
