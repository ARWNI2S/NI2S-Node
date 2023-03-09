using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public interface ISessionEventHost
    {
        ValueTask HandleSessionConnectedEvent(Session session);

        ValueTask HandleSessionClosedEvent(Session session, CloseReason reason);
    }
}