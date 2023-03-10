using System.Net.Sockets;

namespace NI2S.Node.Tests
{
    class DerivedNetworkStream : NetworkStream
    {
        public DerivedNetworkStream(Socket socket)
            : this(socket, false)
        {

        }

        public DerivedNetworkStream(Socket socket, bool ownSocket)
            : base(socket, ownSocket)
        {

        }

        public new Socket Socket
        {
            get { return base.Socket; }
        }
    }
}