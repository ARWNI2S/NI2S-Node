using ARWNI2S.Node.Core.Network.Protocol;
using System.Buffers;

namespace ARWNI2S.Node.Core.Network.Pipeline
{
    internal class ClientPipelineFilter : NI2SPacketFilter
    {
        public override NI2SProtoPacket Filter(ref SequenceReader<byte> reader)
        {
            throw new NotImplementedException();
        }
    }
}