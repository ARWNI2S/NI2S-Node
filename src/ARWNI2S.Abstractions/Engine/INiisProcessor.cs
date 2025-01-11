// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Lifecycle;
using Orleans;

namespace ARWNI2S.Engine
{
    public interface INiisProcessor : ILifecycleParticipant<IEngineLifecycle>
    {
    }
}
