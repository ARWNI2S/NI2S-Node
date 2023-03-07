using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NI2S.Node;
using System.Net;
using System.Net.Sockets;

namespace SuperSocket
{
    public static class SuperSocketExtensions
    {
        public static async ValueTask<bool> ActiveConnect(this IServer server, EndPoint remoteEndpoint)
        {
            var socket = new Socket(remoteEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await socket.ConnectAsync(remoteEndpoint);
                await ((IChannelRegister)server).RegisterChannel(socket);
                return true;
            }
            catch (Exception e)
            {
                var loggerFactory = server.ServiceProvider.GetService<ILoggerFactory>();

                if (loggerFactory != null)
                    loggerFactory.CreateLogger(nameof(ActiveConnect)).LogError(e, $"Failed to connect to {remoteEndpoint}");

                return false;
            }
        }

        public static ILogger GetDefaultLogger(this IAppSession session)
        {
            return ((ILoggerAccessor)session.Server!).Logger;
        }

        public static ServerOptions AddListener(this ServerOptions serverOptions, ListenOptions listener)
        {
            var listeners = serverOptions.Listeners ??= new List<ListenOptions>();
            listeners.Add(listener);
            return serverOptions;
        }
    }
}
