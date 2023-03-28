using Microsoft.Extensions.Logging;
using NI2S.Node.Configuration;

namespace NI2S.Node.Protocol.Channel
{
    public interface IChannelCreatorFactory
    {
        IChannelCreator CreateChannelCreator<TPackageInfo>(ListenOptions? options, ChannelOptions channelOptions, ILoggerFactory loggerFactory, object pipelineFilterFactory);
    }
}