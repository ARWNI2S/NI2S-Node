﻿using System.Net;

namespace NI2S.Node.Client.Proxy
{
    public abstract class ProxyConnectorBase : ConnectorBase
    {
        private readonly EndPoint _proxyEndPoint;

        public ProxyConnectorBase(EndPoint proxyEndPoint)
        {
            _proxyEndPoint = proxyEndPoint;
        }

        protected abstract ValueTask<ConnectState?> ConnectProxyAsync(EndPoint remoteEndPoint, ConnectState? state, CancellationToken cancellationToken);

        protected override async ValueTask<ConnectState?> ConnectAsync(EndPoint remoteEndPoint, ConnectState? state, CancellationToken cancellationToken)
        {
            var socketConnector = new SocketConnector() as IConnector;
            var proxyEndPoint = _proxyEndPoint;

            ConnectState result;

            try
            {
                result = await socketConnector.ConnectAsync(proxyEndPoint, null, cancellationToken);

                if (!result.Result)
                    return result;
            }
            catch (Exception e)
            {
                return new ConnectState
                {
                    Result = false,
                    Exception = e
                };
            }

            return await ConnectProxyAsync(remoteEndPoint, state, cancellationToken);
        }
    }
}
