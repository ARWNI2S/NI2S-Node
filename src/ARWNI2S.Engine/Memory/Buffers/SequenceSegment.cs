using System.Buffers;

namespace ARWNI2S.Engine.Memory.Buffers
{
    public class SequenceSegment : ReadOnlySequenceSegment<byte>, IDisposable
    {
        private bool disposedValue;

        private byte[] _pooledBuffer;

        private bool _pooled = false;

        public SequenceSegment(byte[] pooledBuffer, int length, bool pooled)
        {
            _pooledBuffer = pooledBuffer;
            _pooled = pooled;
            Memory = new ArraySegment<byte>(pooledBuffer, 0, length);
        }

        public SequenceSegment(byte[] pooledBuffer, int length)
            : this(pooledBuffer, length, true)
        {

        }

        public SequenceSegment(ReadOnlyMemory<byte> memory)
        {
            Memory = memory;
        }

        public SequenceSegment SetNext(SequenceSegment segment)
        {
            segment.RunningIndex = RunningIndex + Memory.Length;
            Next = segment;
            return segment;
        }

        public static SequenceSegment CopyFrom(ReadOnlyMemory<byte> memory)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(memory.Length);
            memory.Span.CopyTo(new Span<byte>(buffer));
            return new SequenceSegment(buffer, memory.Length);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_pooled && _pooledBuffer != null)
                        ArrayPool<byte>.Shared.Return(_pooledBuffer);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
