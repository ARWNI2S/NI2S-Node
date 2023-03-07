using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using System.Buffers;

namespace NI2S.Node.Benchmarks
{
    public class TransparentPipeChannel<TPackageInfo> : PipeChannel<TPackageInfo>
    {
        private TaskCompletionSource<int> _tcs;
        private Task<int> _channelTask;

        public TransparentPipeChannel(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options)
            : base(pipelineFilter, options)
        {
            _tcs = new TaskCompletionSource<int>();
            _channelTask = _tcs.Task;
        }

        public override ValueTask CloseAsync(CloseReason closeReason)
        {
            _tcs.SetResult(0);
            return base.CloseAsync(closeReason);
        }

        protected override void Close()
        {

        }

        protected override async ValueTask<int> FillPipeWithDataAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            await _channelTask;
            return 0;
        }

        protected override ValueTask<int> SendOverIOAsync(ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
        {
            return new ValueTask<int>((int)buffer.Length);
        }
    }
}