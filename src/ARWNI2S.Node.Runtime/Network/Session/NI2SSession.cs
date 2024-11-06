using SuperSocket.Connection;
using SuperSocket.Server;

namespace ARWNI2S.Runtime.Network.Session
{
    public class NI2SSession : AppSession
    {
        public override ValueTask CloseAsync()
        {
            return base.CloseAsync();
        }

        public override ValueTask CloseAsync(CloseReason reason)
        {
            return base.CloseAsync(reason);
        }

        protected override ValueTask OnSessionClosedAsync(CloseEventArgs e)
        {
            return base.OnSessionClosedAsync(e);
        }

        protected override ValueTask OnSessionConnectedAsync()
        {
            return base.OnSessionConnectedAsync();
        }

        protected override void Reset()
        {
            base.Reset();
        }
    }
}
