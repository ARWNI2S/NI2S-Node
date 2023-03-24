using NI2S.Node.Configuration.Options;
using NI2S.Node.Networking;
using NI2S.Node.Networking.Channel;
using System;
using System.IO;
using System.Net.Sockets;

namespace NI2S.Node.Client
{
    public class ConnectState
    {
        public ConnectState()
        {

        }

        private ConnectState(bool cancelled)
        {
            Cancelled = cancelled;
        }

        public bool Result { get; set; }

        public bool Cancelled { get; private set; }

        public Exception Exception { get; set; }

        public Socket Socket { get; set; }

        public Stream Stream { get; set; }

        public static readonly ConnectState CancelledState = new(false);

        public IChannel<TReceivePackage> CreateChannel<TReceivePackage>(IPipelineFilter<TReceivePackage> pipelineFilter, ChannelOptions channelOptions)
            where TReceivePackage : class
        {
            if (Socket == null) { throw new InvalidOperationException($"{nameof(Socket)} is null"); }

            if (Stream != null)
            {
                if (Socket.RemoteEndPoint == null) { throw new InvalidOperationException($"{nameof(Socket.RemoteEndPoint)} is null"); }
                if (Socket.LocalEndPoint == null) { throw new InvalidOperationException($"{nameof(Socket.LocalEndPoint)} is null"); }

                return new StreamPipeChannel<TReceivePackage>(Stream, Socket.RemoteEndPoint, Socket.LocalEndPoint, pipelineFilter, channelOptions);
            }
            else
            {
                return new TcpPipeChannel<TReceivePackage>(Socket, pipelineFilter, channelOptions);
            }
        }
    }
}