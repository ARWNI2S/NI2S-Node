using System;
using System.Net;
using System.Threading.Tasks;

namespace NI2S.Node
{
    public static class Extensions
    {
        public static void DoNotAwait(this Task task)
        {

        }

        public static void DoNotAwait(this ValueTask task)
        {

        }

        public static IPEndPoint GetListenEndPoint(this ListenOptions listenOptions)
        {
            if (listenOptions.Ip == null) throw new InvalidOperationException(nameof(listenOptions.Ip));

            var ip = listenOptions.Ip;
            var port = listenOptions.Port;

            IPAddress ipAddress;

            if ("any".Equals(ip, StringComparison.OrdinalIgnoreCase))
            {
                ipAddress = IPAddress.Any;
            }
            else if ("IpV6Any".Equals(ip, StringComparison.OrdinalIgnoreCase))
            {
                ipAddress = IPAddress.IPv6Any;
            }
            else
            {
                ipAddress = IPAddress.Parse(ip);
            }

            return new IPEndPoint(ipAddress, port);
        }
    }
}