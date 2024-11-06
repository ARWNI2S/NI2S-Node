
using SuperSocket.Connection;

namespace ARWNI2S.Node.Core.Network
{
    public interface INetworkContext
    {
        IServiceProvider ContextServices { get; }

        NI2SRequest Request { get; }

        IConnection Connection { get; }

        NI2SResponse Response { get; }

        Dictionary<string, object> Items { get; }
    }
}