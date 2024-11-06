using SuperSocket.ProtoBase;
using System.Buffers;

namespace ARWNI2S.Node.Core.Network.Protocol
{
    public class NI2SPacketDecoder : IPackageDecoder<NI2SProtoPacket>
    {
        public NI2SProtoPacket Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            throw new NotImplementedException();
        }
    }
}
