﻿using NI2S.Node.Configuration.Options;
using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Networking.Channel
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