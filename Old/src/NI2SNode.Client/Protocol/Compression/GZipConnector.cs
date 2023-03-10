using NI2S.Node.Protocol.Compression;
using System.Net;
using System.Net.Sockets;

namespace NI2S.Node.Client
{
    public class GZipConnector : ConnectorBase
    {
        public GZipConnector(IConnector nextConnector)
            : base(nextConnector)
        {
        }
        public GZipConnector()
            : base()
        {
        }

        protected override ValueTask<ConnectState?> ConnectAsync(EndPoint remoteEndPoint, ConnectState? state, CancellationToken cancellationToken)
        {
            var stream = state?.Stream;

            if (stream == null)
            {
                if (state!.Socket != null)
                {
                    stream = new NetworkStream(state.Socket, true);
                }
                else
                {
                    throw new Exception("Stream from previous connector is null.");
                }
            }

            try
            {
                var gzipStream = new GZipReadWriteStream(stream, true);
                state!.Stream = gzipStream;
                return new ValueTask<ConnectState?>(state);
            }
            catch (Exception e)
            {
                return new ValueTask<ConnectState?>(new ConnectState
                {
                    Result = false,
                    Exception = e
                });
            }
        }
    }
}
