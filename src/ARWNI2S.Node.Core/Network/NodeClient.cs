using ARWNI2S.Node.Core.Network.Client;
using ARWNI2S.Node.Core.Network.Pipeline;
using ARWNI2S.Node.Core.Network.Protocol;
using Microsoft.Extensions.Logging;
using SuperSocket.Connection;

namespace ARWNI2S.Node.Core.Network
{
    public class DefaultNodeClient : NodeClient
    {
        public DefaultNodeClient(ILogger logger = null)
            : this(new ConnectionOptions { Logger = logger}) { }
        
        public DefaultNodeClient(ConnectionOptions options)
            : base(new PipelineFilterRoot(), new NI2SPacketEncoder(), options)
        {
        }

        protected DefaultNodeClient(NI2SPacketEncoder packageEncoder) : base(packageEncoder)
        {
        }
    }

    public class NodeClient : NodeClient<NI2SProtoPacket, NI2SProtoPacket>
    {
        protected NodeClient(NI2SPacketEncoder packageEncoder)
            : base(packageEncoder) { }

        public NodeClient(NI2SPacketFilter pipelineFilter, NI2SPacketEncoder packageEncoder, ILogger logger = null)
            : this(pipelineFilter, packageEncoder, new ConnectionOptions { Logger = logger }) { }

        public NodeClient(NI2SPacketFilter pipelineFilter, NI2SPacketEncoder packageEncoder, ConnectionOptions options)
            : base(pipelineFilter, packageEncoder, options) { }

    }
}
