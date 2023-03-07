using NI2S.Node;
using NI2S.Node.Protocol.Channel;

namespace SuperSocket.Server
{
    public class SessionHandlers
    {
        public Func<IAppSession, ValueTask>? Connected { get; set; }

        public Func<IAppSession, CloseEventArgs, ValueTask>? Closed { get; set; }
    }
}