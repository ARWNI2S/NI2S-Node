using NI2S.Node.Hosting;
using NI2S.Node.Protocol.Channel.Compression;

namespace NI2S.Node.Protocol.Compression
{
    public static class HostBuilderExtensions
    {

        // move to extensions
        public static INodeHostBuilderOld? UseGZip(this INodeHostBuilderOld hostBuilder)
        {
            return hostBuilder.UseChannelCreatorFactory<GZipTcpChannelCreatorFactory>();
        }

    }
}
