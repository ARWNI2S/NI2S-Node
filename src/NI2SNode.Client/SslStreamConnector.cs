using NI2S.Node.Client.Resources;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;

namespace NI2S.Node.Client
{
    public class SslStreamConnector : ConnectorBase
    {
        public SslClientAuthenticationOptions Options { get; private set; }

        public SslStreamConnector(SslClientAuthenticationOptions options)
            : base()
        {
            Options = options;
        }

        public SslStreamConnector(SslClientAuthenticationOptions options, IConnector nextConnector)
            : base(nextConnector)
        {
            Options = options;
        }

        protected override async ValueTask<ConnectState?> ConnectAsync(EndPoint remoteEndPoint, ConnectState? state, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(state, nameof(state));

            var socket = state.Socket;

            if (socket == null)
                throw new InvalidOperationException(LocalizedStrings.Error_SocketIsNull);

            var targetHost = Options.TargetHost;

            if (string.IsNullOrEmpty(targetHost))
            {
                if (remoteEndPoint is DnsEndPoint remoteDnsEndPoint)
                    targetHost = remoteDnsEndPoint.Host;
                else if (remoteEndPoint is IPEndPoint remoteIPEndPoint)
                    targetHost = remoteIPEndPoint.Address.ToString();

                Options.TargetHost = targetHost;
            }


            try
            {
                var stream = new SslStream(new NetworkStream(socket, true), false);
                await stream.AuthenticateAsClientAsync(Options, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return ConnectState.CancelledState;

                state.Stream = stream;
                return state;
            }
            catch (Exception e)
            {
                return new ConnectState
                {
                    Result = false,
                    Exception = e
                };
            }
        }
    }
}
