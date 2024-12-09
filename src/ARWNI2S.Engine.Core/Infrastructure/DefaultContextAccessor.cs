using ARWNI2S.Engine;

namespace ARWNI2S.Core.Infrastructure
{
    internal class DefaultContextAccessor : INiisContextAccessor
    {
        private static readonly AsyncLocal<NI2SContextHolder> _niisContextCurrent = new();

        /// <inheritdoc/>
        public INiisContext EngineContext
        {
            get
            {
                return _niisContextCurrent.Value?.EngineContext;
            }
            set
            {
                var holder = _niisContextCurrent.Value;
                if (holder != null)
                {
                    // Clear current EngineContext trapped in the AsyncLocals, as its done.
                    holder.EngineContext = null;
                }

                if (value != null)
                {
                    // Use an object indirection to hold the EngineContext in the AsyncLocal,
                    // so it can be cleared in all ExecutionContexts when its cleared.
                    _niisContextCurrent.Value = new NI2SContextHolder { EngineContext = value };
                }
            }
        }


        private sealed class NI2SContextHolder
        {
            public INiisContext EngineContext;
        }
    }
}
