using Microsoft.Extensions.Logging;
using NI2S.Node.Async;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Protocol.Udp;
using System.Net;
using System.Net.Sockets;

namespace NI2S.Node.Protocol.Channel.Udp
{
    class UdpChannelCreatorFactory : IChannelCreatorFactory
    {
        private readonly IUdpSessionIdentifierProvider _udpSessionIdentifierProvider;

        private readonly IAsyncSessionContainer _sessionContainer;

        public UdpChannelCreatorFactory(IUdpSessionIdentifierProvider udpSessionIdentifierProvider, IAsyncSessionContainer sessionContainer)
        {
            _udpSessionIdentifierProvider = udpSessionIdentifierProvider;
            _sessionContainer = sessionContainer;
        }

        public IChannelCreator CreateChannelCreator<TPackageInfo>(ListenOptions? options, ChannelOptions channelOptions, ILoggerFactory loggerFactory, object pipelineFilterFactory)
        {
            var filterFactory = pipelineFilterFactory as IPipelineFilterFactory<TPackageInfo>;
            channelOptions.Logger = loggerFactory.CreateLogger(nameof(IChannel));
            var channelFactoryLogger = loggerFactory.CreateLogger(nameof(UdpChannelCreator));

            var channelFactory = new Func<Socket, IPEndPoint, string, ValueTask<IVirtualChannel>>((s, re, id) =>
            {
                var filter = filterFactory?.Create(s);
                return new ValueTask<IVirtualChannel>(new UdpPipeChannel<TPackageInfo>(s, filter!, channelOptions, re, id));
            });

            return new UdpChannelCreator(options!, channelOptions, channelFactory, channelFactoryLogger, _udpSessionIdentifierProvider, _sessionContainer);
        }
    }
}