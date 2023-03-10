using System;
using System.Linq;
using NI2S.Node.Command;
using NI2S.Node.Protocol;
using NI2S.Node.WebSocket;

namespace NI2S.Node.Tests.WebSocket
{
    class StringPackageConverter : IPackageMapper<WebSocketPackage, StringPackageInfo>
    {
        public StringPackageInfo Map(WebSocketPackage package)
        {
            var pack = new StringPackageInfo();
            var arr = package.Message.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
            pack.Key = arr[0];
            pack.Parameters = arr.Skip(1).ToArray();
            return pack;
        }
    }
}