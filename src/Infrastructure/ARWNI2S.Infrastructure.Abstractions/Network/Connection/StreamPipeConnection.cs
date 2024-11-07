using System.Buffers;
using System.Net.Sockets;
using System.Net;

namespace ARWNI2S.Infrastructure.Network.Connection
{
    public class StreamPipeConnection : PipeConnection, IStreamConnection
    {
        private Stream _stream;

        public StreamPipeConnection(Stream stream, EndPoint remoteEndPoint, ConnectionOptions options)
            : this(stream, remoteEndPoint, null, options)
        {

        }

        public StreamPipeConnection(Stream stream, EndPoint remoteEndPoint, EndPoint localEndPoint, ConnectionOptions options)
            : base(options)
        {
            _stream = stream;
            RemoteEndPoint = remoteEndPoint;
            LocalEndPoint = localEndPoint;
        }

        protected override void Close()
        {
            _stream.Close();
        }


        protected override void OnClosed()
        {
            _stream = null;
            base.OnClosed();
        }

        protected override async ValueTask<int> FillPipeWithDataAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            return await _stream.ReadAsync(memory, cancellationToken).ConfigureAwait(false);
        }

        protected override async ValueTask<int> SendOverIOAsync(ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
        {
            var total = 0;

            foreach (var data in buffer)
            {
                await _stream.WriteAsync(data, cancellationToken).ConfigureAwait(false);
                total += data.Length;
            }

            await _stream.FlushAsync(cancellationToken).ConfigureAwait(false);
            return total;
        }

        protected override bool IsIgnorableException(Exception e)
        {
            if (base.IsIgnorableException(e))
                return true;

            if (e is SocketException se)
            {
                if (se.IsIgnorableSocketException())
                    return true;
            }

            return false;
        }

        Stream IStreamConnection.Stream
        {
            get { return _stream; }
        }
    }
}