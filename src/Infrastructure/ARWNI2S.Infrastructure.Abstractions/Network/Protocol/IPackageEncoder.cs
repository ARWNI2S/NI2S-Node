using System.Buffers;

namespace ARWNI2S.Infrastructure.Network.Protocol
{
    public interface IPackageEncoder<in TPackageInfo>
    {
        int Encode(IBufferWriter<byte> writer, TPackageInfo pack);
    }
}