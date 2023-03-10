using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NI2S.Node.Async;
using NI2S.Node.Hosting;
using NI2S.Node.Middleware;
using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Channel.Udp;
using NI2S.Node.Protocol.Session;
using NI2S.Node.Protocol.Udp;

namespace NI2S.Node
{
    public static class UdpServerHostBuilderExtensions
    {
        public static INodeHostBuilderOld<TReceivePackage> UseUdp<TReceivePackage>(this INodeHostBuilderOld<TReceivePackage> hostBuilder)
        {
            return (INodeHostBuilderOld<TReceivePackage>)((INodeHostBuilderOld)hostBuilder).UseUdp();
        }

        public static INodeHostBuilderOld UseUdp(this INodeHostBuilderOld hostBuilder)
        {
            return ((INodeHostBuilderOld)hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IChannelCreatorFactory, UdpChannelCreatorFactory>();
            }))
            .ConfigureSupplementServices((context, services) =>
            {
                if (!services.Any(s => s.ServiceType == typeof(IUdpSessionIdentifierProvider)))
                {
                    services.AddSingleton<IUdpSessionIdentifierProvider, IPAddressUdpSessionIdentifierProvider>();
                }

                if (!services.Any(s => s.ServiceType == typeof(IAsyncSessionContainer)))
                {
                    services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, InProcSessionContainerMiddleware>(s => s.GetRequiredService<InProcSessionContainerMiddleware>()));
                    services.AddSingleton<InProcSessionContainerMiddleware>();
                    services.AddSingleton<ISessionContainer>((s) => s.GetRequiredService<InProcSessionContainerMiddleware>());
                    services.AddSingleton((s) => s.GetRequiredService<ISessionContainer>().ToAsyncSessionContainer());
                }
            });
        }
    }
}