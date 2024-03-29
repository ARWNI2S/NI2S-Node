﻿using System.Net;

namespace NI2S.Node.Client
{
    public interface IConnector
    {
        ValueTask<ConnectState> ConnectAsync(EndPoint remoteEndPoint, ConnectState? state = null, CancellationToken cancellationToken = default);

        IConnector? NextConnector { get; }
    }
}