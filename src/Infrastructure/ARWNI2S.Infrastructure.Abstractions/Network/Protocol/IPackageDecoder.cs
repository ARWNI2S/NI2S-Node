using System.Buffers;

namespace ARWNI2S.Infrastructure.Network.Protocol
{
    public interface IPackageDecoder<out TPackageInfo>
    {
        TPackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context);
    }
}