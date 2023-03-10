using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace NI2S.Node.Diagnostics.Environment
{
    /// <summary>
    /// This class is designed for detect platform attribute in runtime
    /// </summary>
    public static class Platform
    {
        static Platform()
        {
            try
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //TODO::
                //socket.IOControl(IOControlCode.KeepAliveValues, null, null);
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

        /// <summary>
        /// Gets a value indicating whether [support socket IO control by code enum].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [support socket IO control by code enum]; otherwise, <c>false</c>.
        /// </value>
        public static bool SupportSocketIOControlByCodeEnum { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is FreeBSD.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is FreeBSD; otherwise, <c>false</c>.
        /// </value>
        public static bool IsFreeBSD { get { return RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD); } }
        /// <summary>
        /// Gets a value indicating whether this instance is Linux.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is Linux; otherwise, <c>false</c>.
        /// </value>
        public static bool IsLinux { get { return RuntimeInformation.IsOSPlatform(OSPlatform.Linux); } }
        /// <summary>
        /// Gets a value indicating whether this instance is OsX.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is OsX; otherwise, <c>false</c>.
        /// </value>
        public static bool IsOsX { get { return RuntimeInformation.IsOSPlatform(OSPlatform.OSX); } }
        /// <summary>
        /// Gets a value indicating whether this instance is Windows.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is Windows; otherwise, <c>false</c>.
        /// </value>
        public static bool IsWindows { get { return RuntimeInformation.IsOSPlatform(OSPlatform.Windows); } }

    }
}
