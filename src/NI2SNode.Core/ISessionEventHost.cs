using NI2S.Node.Protocol.Channel;

namespace NI2S.Node.Server
{
    public interface ISessionEventHost
    {
        ValueTask HandleSessionConnectedEvent(AppSession session);

        ValueTask HandleSessionClosedEvent(AppSession session, CloseReason reason);
    }
}