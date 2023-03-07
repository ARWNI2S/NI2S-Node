using System.Buffers;

namespace NI2S.Node.Protocol
{
    public interface IPackagePartReader<TPackageInfo>
    {
        bool Process(TPackageInfo package, object? filterContext, ref SequenceReader<byte> reader, out IPackagePartReader<TPackageInfo> nextPartReader, out bool needMoreData);
    }
}