using NI2S.Node.Async;
using NI2S.Node.Protocol.Channel;
using System.Net;

namespace NI2S.Node.Protocol.Session
{
    public interface ISession
    {
        string SessionID { get; }

        DateTimeOffset StartTime { get; }

        DateTimeOffset LastActiveTime { get; }

        IChannel? Channel { get; }

        EndPoint? RemoteEndPoint { get; }

        EndPoint? LocalEndPoint { get; }

        ValueTask SendAsync(ReadOnlyMemory<byte> data);

        ValueTask SendAsync<TPackage>(IPackageEncoder<TPackage> packageEncoder, TPackage package);

        ValueTask CloseAsync(CloseReason reason);

        INodeInfo? Server { get; }

        event AsyncEventHandler Connected;

        event AsyncEventHandler<CloseEventArgs> Closed;

        object? DataContext { get; set; }

        void Initialize(INodeInfo server, IChannel channel);

        object? this[object name] { get; set; }

        SessionState State { get; }

        void Reset();
    }
}