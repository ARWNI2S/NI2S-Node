using ARWNI2S.Engine;
using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Protocol;

namespace ARWNI2S.Runtime
{
    internal class RuntimeContextAccessor : PackageHandlingContextAccessor<NI2SProtoPacket>, INetworkContextAccessor
    {
        private static readonly AsyncLocal<NetworkContextHolder> _engineContextCurrent = new();

        /// <inheritdoc/>
        public INetworkContext NetworkContext
        {
            get
            {
                return _engineContextCurrent.Value?.Context;
            }
            set
            {
                var holder = _engineContextCurrent.Value;
                if (holder != null)
                {
                    // Clear current EngineContext trapped in the AsyncLocals, as its done.
                    holder.Context = null;
                }

                if (value != null)
                {
                    // Use an object indirection to hold the EngineContext in the AsyncLocal,
                    // so it can be cleared in all ExecutionContexts when its cleared.
                    _engineContextCurrent.Value = new NetworkContextHolder { Context = value };
                }
            }
        }


        private sealed class NetworkContextHolder
        {
            public INetworkContext Context;
        }
    }
}
