﻿using System.IO.Pipelines;
using System.Net;

namespace NI2S.Node.Protocol.Channel
{
    public abstract class ChannelBase<TPackageInfo> : IChannel<TPackageInfo>, IChannel
    {
        public abstract void Start();

        public abstract IAsyncEnumerable<TPackageInfo> RunAsync();

        public abstract ValueTask SendAsync(ReadOnlyMemory<byte> buffer);

        public abstract ValueTask SendAsync<TPackage>(IPackageEncoder<TPackage> packageEncoder, TPackage package);

        public abstract ValueTask SendAsync(Action<PipeWriter> write);

        public bool IsClosed { get; private set; }

        public EndPoint RemoteEndPoint { get; protected set; } = default!;

        public EndPoint? LocalEndPoint { get; protected set; }

        public CloseReason? CloseReason { get; protected set; }

        public DateTimeOffset LastActiveTime { get; protected set; } = DateTimeOffset.Now;

        protected virtual void OnClosed()
        {
            IsClosed = true;

            var closed = Closed;

            if (closed == null)
                return;

            if (Interlocked.CompareExchange(ref Closed, null, closed) != closed)
                return;

            var closeReason = CloseReason ?? Channel.CloseReason.Unknown;

            closed.Invoke(this, new CloseEventArgs(closeReason));
        }

        public event EventHandler<CloseEventArgs>? Closed;

        public abstract ValueTask CloseAsync(CloseReason closeReason);

        public abstract ValueTask DetachAsync();
    }
}