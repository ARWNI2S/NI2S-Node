﻿using ARWNI2S.Infrastructure;

namespace ARWNI2S.Runtime
{
    internal class RuntimeContextAccessor : /*PackageHandlingContextAccessor<NI2SProtoPacket>,*/ IContextAccessor
    {
        private static readonly AsyncLocal<ContextHolder> _engineContextCurrent = new();

        /// <inheritdoc/>
        public IContext Context
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
                    _engineContextCurrent.Value = new ContextHolder { Context = value };
                }
            }
        }


        private sealed class ContextHolder
        {
            public IContext Context;
        }
    }
}
