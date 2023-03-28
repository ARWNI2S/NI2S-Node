using System.Net.Sockets;

namespace NI2S.Node.Networking.Channel
{
    public static class SocketExtensions
    {
        public static bool IsIgnorableSocketException(this SocketException se)
        {
            switch (se.SocketErrorCode)
            {
                case (SocketError.OperationAborted):
                case (SocketError.ConnectionReset):
                case (SocketError.TimedOut):
                case (SocketError.NetworkReset):
                    return true;
                default:
                    return false;
            }
        }
    }
}
