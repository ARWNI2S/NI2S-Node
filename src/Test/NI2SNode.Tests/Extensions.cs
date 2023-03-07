using System.Net;

namespace NI2S.Node.Tests
{
    public static class Extensions
    {
        internal static IPEndPoint GetServerEndPoint(this IHostConfigurator hostConfigurator)
        {
            return new IPEndPoint(IPAddress.Loopback, hostConfigurator.Listener.Port);
        }
    }
}
