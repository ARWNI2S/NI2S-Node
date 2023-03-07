using System.Net;

namespace NI2S.Node.Protocol.Udp
{
    public interface IUdpSessionIdentifierProvider
    {
        string GetSessionIdentifier(IPEndPoint remoteEndPoint, ArraySegment<byte>? data = null);
    }
}