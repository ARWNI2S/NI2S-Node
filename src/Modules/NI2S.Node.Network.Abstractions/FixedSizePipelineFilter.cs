using System.Buffers;

namespace NI2S.Node.Networking
{
    public class FixedSizePipelineFilter<TPackageInfo> : PipelineFilterBase<TPackageInfo>
        where TPackageInfo : class
    {
        private readonly int _size;

        protected FixedSizePipelineFilter(int size)
        {
            _size = size;
        }

        public override TPackageInfo? Filter(ref SequenceReader<byte> reader)
        {
            if (reader.Length < _size)
                return null;

            var pack = reader.Sequence.Slice(0, _size);

            try
            {
                return DecodePackage(ref pack);
            }
            finally
            {
                reader.Advance(_size);
            }
        }
    }
}
