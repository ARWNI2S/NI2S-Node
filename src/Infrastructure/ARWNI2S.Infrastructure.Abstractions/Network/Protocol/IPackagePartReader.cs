using System.Buffers;

namespace ARWNI2S.Infrastructure.Network.Protocol
{
    public interface IPackagePartReader<TPackageInfo>
    {
        bool Process(TPackageInfo package, object filterContext, ref SequenceReader<byte> reader, out IPackagePartReader<TPackageInfo> nextPartReader, out bool needMoreData);
    }
}