using System.Net;

namespace NI2S.Node.Client.Proxy
{
    public class Socks4Connector : ConnectorBase
    {
        protected override ValueTask<ConnectState?> ConnectAsync(EndPoint remoteEndPoint, ConnectState? state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
