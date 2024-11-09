using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Configuration.Options;
using ARWNI2S.Engine.Network.Connection;
using ARWNI2S.Engine.Network.Session;
using ARWNI2S.Infrastructure.Logging;
using ARWNI2S.Runtime.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace ARWNI2S.Runtime
{
    public static class NodeServerExtensions
    {
        public static async ValueTask<bool> ActiveConnect(this IServer server, EndPoint remoteEndpoint)
        {
            var socket = new Socket(remoteEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await socket.ConnectAsync(remoteEndpoint);
                await (server as IConnectionRegister).RegisterConnection(socket);
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

        public static ILogger GetDefaultLogger(this INodeSession session)
        {
            return (session.Server as ILoggerAccessor)?.Logger;
        }

        public static ServerOptions AddListener(this ServerOptions serverOptions, ListenOptions listener)
        {
            var listeners = serverOptions.Listeners;

            if (listeners == null)
                listeners = serverOptions.Listeners = [];

            listeners.Add(listener);
            return serverOptions;
        }

        public static void AdaptMultipleServerHost(this IHost host)
        {
            var services = host.Services;
            services.GetService<MultipleServerHostBuilder>()?.AdaptMultipleServerHost(services);
        }
    }
}