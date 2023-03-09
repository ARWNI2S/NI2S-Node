using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Client;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Compression;
using System.Net.Sockets;
using System.Security.Authentication;

namespace NI2S.Node.Tests
{
    public class GzipHostConfigurator : TcpHostConfigurator
    {
        public GzipHostConfigurator()
        {
            WebSocketSchema = "wss";
            IsSecure = false;
        }

        public override void Configure(INodeHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((ctx, services) =>
            {
                services.Configure<ServerOptions>((options) =>
                {
                    var listener = options.Listeners[0];

                });
            });
            hostBuilder.UseGZip();

            base.Configure(hostBuilder);
        }
        public override ValueTask<Stream> GetClientStream(Socket socket)
        {
            Stream stream = new GZipReadWriteStream(new NetworkStream(socket, false), false);
            return new ValueTask<Stream>(stream);
        }

        protected virtual SslProtocols GetServerEnabledSslProtocols()
        {
            return SslProtocols.Tls13 | SslProtocols.Tls12;
        }

        protected virtual SslProtocols GetClientEnabledSslProtocols()
        {
            return SslProtocols.Tls13 | SslProtocols.Tls12;
        }

        public override INodeClient<TPackageInfo> ConfigureNodeClient<TPackageInfo>(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options) where TPackageInfo : class
        {
            return new GZipNodeClient<TPackageInfo>(pipelineFilter, options);
        }
    }

}