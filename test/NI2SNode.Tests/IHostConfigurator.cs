using NI2S.Node.Client;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using NI2S.Node.Configuration.Options;
using System.Net.Sockets;
using System.Text;

namespace NI2S.Node.Tests
{
    public interface IHostConfigurator
    {
        void Configure(INodeHostBuilder hostBuilder);

        ValueTask KeepSequence();

        Socket CreateClient();

        ValueTask<Stream> GetClientStream(Socket socket);

        TextReader GetStreamReader(Stream stream, Encoding encoding);

        string WebSocketSchema { get; }

        bool IsSecure { get; }

        ListenOptions Listener { get; }

        INodeClient<TPackageInfo> ConfigureNodeClient<TPackageInfo>(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options)
            where TPackageInfo : class;
    }
}