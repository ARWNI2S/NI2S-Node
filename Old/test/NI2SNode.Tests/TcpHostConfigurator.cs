using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Client;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using System.Net.Sockets;
using System.Text;

namespace NI2S.Node.Tests
{
    public abstract class TcpHostConfigurator : IHostConfigurator
    {
        public string WebSocketSchema { get; protected set; }

        public bool IsSecure { get; protected set; }

        public ListenOptions Listener { get; private set; }

        public virtual void Configure(INodeHostBuilderOld hostBuilder)
        {
            hostBuilder.ConfigureServices((ctx, services) =>
            {
                services.Configure<ServerOptions>((options) =>
                {
                    var listener = options.Listeners[0];
                    Listener = listener;
                });
            });
        }

        public Socket CreateClient()
        {
            var serverAddress = this.GetServerEndPoint();
            var socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(serverAddress);
            return socket;
        }


        public TextReader GetStreamReader(Stream stream, Encoding encoding)
        {
            return new StreamReader(stream, encoding, true);
        }

        public abstract INodeClient<TPackageInfo> ConfigureNodeClient<TPackageInfo>(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options)
            where TPackageInfo : class;

        public abstract ValueTask<Stream> GetClientStream(Socket socket);

        public ValueTask KeepSequence()
        {
            return new ValueTask();
        }
    }
}