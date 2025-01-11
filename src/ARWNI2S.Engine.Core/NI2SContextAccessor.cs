// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine
{
    internal class NI2SContextAccessor : INiisContextAccessor
    {
        private static readonly AsyncLocal<NI2SContextHolder> _niisContextCurrent = new();

        /// <inheritdoc/>
        public INiisContext NI2SContext
        {
            get
            {
                return _niisContextCurrent.Value?.Context;
            }
            set
            {
                var holder = _niisContextCurrent.Value;
                if (holder != null)
                {
                    // Clear current HttpContext trapped in the AsyncLocals, as its done.
                    holder.Context = null;
                }

                if (value != null)
                {
                    // Use an object indirection to hold the HttpContext in the AsyncLocal,
                    // so it can be cleared in all ExecutionContexts when its cleared.
                    _niisContextCurrent.Value = new NI2SContextHolder { Context = value };
                }
            }
        }

        private sealed class NI2SContextHolder
        {
            public INiisContext Context;
        }
    }
}
