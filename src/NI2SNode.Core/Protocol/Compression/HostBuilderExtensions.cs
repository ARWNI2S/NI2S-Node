using NI2S.Node.Hosting;
using NI2S.Node.Protocol.Channel.Compression;

namespace NI2S.Node.Protocol.Compression
{
    public static class HostBuilderExtensions
    {

        // move to extensions
        public static INodeHostBuilder? UseGZip(this INodeHostBuilder hostBuilder)
        {
            return hostBuilder.UseChannelCreatorFactory<GZipTcpChannelCreatorFactory>();
        }

    }
}
