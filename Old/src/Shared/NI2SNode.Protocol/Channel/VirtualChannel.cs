using System.IO.Pipelines;
using NI2S.Node.Configuration.Options;

namespace NI2S.Node.Protocol.Channel
{
    public abstract class VirtualChannel<TPackageInfo> : PipeChannel<TPackageInfo>, IVirtualChannel
    {
        public VirtualChannel(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options)
            : base(pipelineFilter, options)
        {

        }

        protected override Task FillPipeAsync(PipeWriter writer)
        {
            return Task.CompletedTask;
        }

        public async ValueTask<FlushResult> WritePipeDataAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            return await In.Writer.WriteAsync(memory, cancellationToken).ConfigureAwait(false);
        }
    }
}