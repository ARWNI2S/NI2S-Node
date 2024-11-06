using System.Net;

namespace ARWNI2S.Node.Core.Network.Client
{
    public interface IConnector
    {
        ValueTask<ConnectState> ConnectAsync(EndPoint remoteEndPoint, ConnectState state = null, CancellationToken cancellationToken = default);

        IConnector NextConnector { get; }
    }
}