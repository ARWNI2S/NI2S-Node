using System.Buffers;

namespace NI2S.Node.Protocol
{
    public interface IPipelineFilter
    {
        void Reset();

        object? Context { get; set; }
    }

    public interface IPipelineFilter<TPackageInfo> : IPipelineFilter
    {

        IPackageDecoder<TPackageInfo>? Decoder { get; set; }

        TPackageInfo? Filter(ref SequenceReader<byte> reader);

        IPipelineFilter<TPackageInfo>? NextFilter { get; }

    }
}
