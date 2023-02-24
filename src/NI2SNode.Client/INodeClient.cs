﻿using NI2S.Node.Protocol;
using System.Net;

namespace NI2S.Node.Client
{
    public interface INodeClient<TReceivePackage, TSendPackage> : IEasyClient<TReceivePackage>
        where TReceivePackage : class
    {
        ValueTask SendAsync(TSendPackage package);
    }


    public interface IEasyClient<TReceivePackage>
        where TReceivePackage : class
    {
        ValueTask<bool> ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken = default);

        ValueTask<TReceivePackage> ReceiveAsync();

        IPEndPoint? LocalEndPoint { get; set; }

        SecurityOptions Security { get; set; }

        void StartReceive();

        ValueTask SendAsync(ReadOnlyMemory<byte> data);

        ValueTask SendAsync<TSendPackage>(IPackageEncoder<TSendPackage> packageEncoder, TSendPackage package);

        event EventHandler? Closed;

        event PackageHandler<TReceivePackage> PackageHandler;

        ValueTask CloseAsync();
    }
}
