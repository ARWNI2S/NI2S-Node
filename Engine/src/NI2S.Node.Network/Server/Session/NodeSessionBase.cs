// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network.Channel;
using NI2S.Network.Server;
using System.Threading.Tasks;

namespace NI2S.Node.Network.Server
{
    public abstract class NodeSessionBase : AppSession, INodeSession
    {
        public SystemSessionType SessionType { get; }

        protected NodeSessionBase(SystemSessionType sessionType)
        {
            SessionType = sessionType;
        }

        protected override void Reset()
        {
            base.Reset();
        }

        protected override ValueTask OnSessionConnectedAsync()
        {
            return base.OnSessionConnectedAsync();
        }

        protected override ValueTask OnSessionClosedAsync(CloseEventArgs e)
        {
            return base.OnSessionClosedAsync(e);
        }

        public override ValueTask CloseAsync()
        {
            return base.CloseAsync();
        }

        public override ValueTask CloseAsync(CloseReason reason)
        {
            return base.CloseAsync(reason);
        }
    }
}
