using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using System.Net.Sockets;

namespace NI2S.Node.Tests
{
    public class UdpChannelStream : Stream
    {
        public UdpPipeChannel<TextPackageInfo> Channel { get; }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new System.NotSupportedException();

        public override long Position { get => throw new System.NotSupportedException(); set => throw new System.NotSupportedException(); }

        public Socket Socket { get; }

        public UdpChannelStream(UdpPipeChannel<TextPackageInfo> channel, Socket socket)
        {
            Channel = channel;
            Socket = socket;
        }

        public override void Flush()
        {

        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new System.NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new System.NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Channel
                .SendAsync((new ArraySegment<byte>(buffer, offset, count)).AsMemory())
                .GetAwaiter()
                .GetResult();
        }

        protected override void Dispose(bool disposing)
        {
            Socket?.Close();
            base.Dispose(disposing);
        }
    }
}