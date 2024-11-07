using System.IO.Pipelines;

namespace ARWNI2S.Infrastructure.Network.Connection
{
    public interface IVirtualConnection : IConnection
    {
        ValueTask<FlushResult> WritePipeDataAsync(Memory<byte> memory, CancellationToken cancellationToken);
    }
}
