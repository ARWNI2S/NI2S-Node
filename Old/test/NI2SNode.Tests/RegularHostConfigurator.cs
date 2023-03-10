using NI2S.Node.Client;
using NI2S.Node.Protocol;
using NI2S.Node.Configuration.Options;
using System.Net.Sockets;

namespace NI2S.Node.Tests
{
    public class RegularHostConfigurator : TcpHostConfigurator
    {
        public RegularHostConfigurator()
        {
            WebSocketSchema = "ws";
        }

        public override INodeClient<TPackageInfo> ConfigureNodeClient<TPackageInfo>(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options) where TPackageInfo : class
        {
            return new NodeClient<TPackageInfo>(pipelineFilter, options);
        }

        public override ValueTask<Stream> GetClientStream(Socket socket)
        {
            return new ValueTask<Stream>(new DerivedNetworkStream(socket, false));
        }
    }
}