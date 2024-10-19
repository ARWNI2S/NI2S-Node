using ARWNI2S.Node.Core.Runtime;

namespace ARWNI2S.Runtime
{
    internal class RuntimeContextAccessor : IRuntimeContextAccessor
    {
        private static readonly AsyncLocal<EngineContextHolder> _engineContextCurrent = new();

        /// <inheritdoc/>
        public IRuntimeContext EngineContext
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
                    _engineContextCurrent.Value = new EngineContextHolder { Context = value };
                }
            }
        }

        private sealed class EngineContextHolder
        {
            public IRuntimeContext Context;
        }
    }
}
