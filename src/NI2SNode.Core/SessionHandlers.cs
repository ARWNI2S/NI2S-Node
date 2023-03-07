using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Server
{
    public class SessionHandlers
    {
        public Func<ISession, ValueTask>? Connected { get; set; }

        public Func<ISession, CloseEventArgs, ValueTask>? Closed { get; set; }
    }
}