using NI2S.Node.Client.Options;
using NI2S.Node.Networking;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Client
{
    public interface INodeClient<TReceivePackage, TSendPackage> : INodeClient<TReceivePackage>
        where TReceivePackage : class
    {
        ValueTask SendAsync(TSendPackage package);
    }


    public interface INodeClient<TReceivePackage>
        where TReceivePackage : class
    {
        ValueTask<bool> ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken = default);

        ValueTask<TReceivePackage> ReceiveAsync();

        IPEndPoint LocalEndPoint { get; set; }

        SecurityOptions Security { get; set; }

        void StartReceive();

        ValueTask SendAsync(ReadOnlyMemory<byte> data);

        ValueTask SendAsync<TSendPackage>(IPackageEncoder<TSendPackage> packageEncoder, TSendPackage package);

        event EventHandler Closed;

        event PackageHandler<TReceivePackage> PackageHandler;

        ValueTask CloseAsync();
    }
}
