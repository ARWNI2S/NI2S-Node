using System.Buffers;

namespace NI2S.Node.Networking
{
    public interface IPackageDecoder<out TPackageInfo>
    {
        TPackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context);
    }
}