using NI2S.Node.Client;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using System.Net.Sockets;

namespace NI2S.Node.Tests
{
    public class RegularHostConfigurator : TcpHostConfigurator
    {
        public RegularHostConfigurator()
        {
            WebSocketSchema = "ws";
        }

        public override IEasyClient<TPackageInfo> ConfigureEasyClient<TPackageInfo>(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options) where TPackageInfo : class
        {
            return new EasyClient<TPackageInfo>(pipelineFilter, options);
        }

        public override ValueTask<Stream> GetClientStream(Socket socket)
        {
            return new ValueTask<Stream>(new DerivedNetworkStream(socket, false));
        }
    }
}