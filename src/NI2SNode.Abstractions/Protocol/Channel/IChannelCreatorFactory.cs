using Microsoft.Extensions.Logging;

namespace NI2S.Node.Protocol.Channel
{
    public interface IChannelCreatorFactory
    {
        IChannelCreator CreateChannelCreator<TPackageInfo>(ListenOptions? options, ChannelOptions channelOptions, ILoggerFactory loggerFactory, object pipelineFilterFactory);
    }
}