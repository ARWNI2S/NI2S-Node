using System.Buffers;

namespace ARWNI2S.Infrastructure.Network.Protocol
{
    public interface IPipelineFilter
    {
        void Reset();

        object Context { get; set; }
    }

    public interface IPipelineFilter<TPackageInfo> : IPipelineFilter
    {

        IPackageDecoder<TPackageInfo> Decoder { get; set; }

        TPackageInfo Filter(ref SequenceReader<byte> reader);

        IPipelineFilter<TPackageInfo> NextFilter { get; }

    }
}