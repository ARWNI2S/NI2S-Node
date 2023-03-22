using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;

namespace NI2S.Node.Infrastructure.Platform
{
    public class NetworkCaps : PlatformInfo<NetworkCaps>
    {
        public object? Empty { get; internal set; }

        public NetworkCaps() : base() { }

        /// <summary>
        /// Gets a value indicating whether [support socket IO control by code enum].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [support socket IO control by code enum]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportSocketIOControlByCodeEnum { get; private set; }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Beign used for 'Validate platform compatibility'")]
        protected override void ReadCapabilities()
        {
            try
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int result = socket.IOControl(IOControlCode.KeepAliveValues, null, null);
                SupportSocketIOControlByCodeEnum = true;
            }
            catch (NotSupportedException)
            {
                SupportSocketIOControlByCodeEnum = false;
            }
            catch (NotImplementedException)
            {
                SupportSocketIOControlByCodeEnum = false;
            }
            catch (Exception)
            {
                SupportSocketIOControlByCodeEnum = true;
            }
        }
    }
}
