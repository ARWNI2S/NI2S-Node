using System.IO.Pipelines;

namespace NI2S.Node.Protocol.Channel
{
    public interface IVirtualChannel : IChannel
    {
        ValueTask<FlushResult> WritePipeDataAsync(Memory<byte> memory, CancellationToken cancellationToken);
    }
}