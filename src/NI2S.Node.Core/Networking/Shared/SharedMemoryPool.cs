using System.Buffers;

namespace NI2S.Node.Networking.Shared
{
    internal sealed class SharedMemoryPool
    {
        public MemoryPool<byte> Pool { get; } = KestrelMemoryPool.Create();
    }
}
