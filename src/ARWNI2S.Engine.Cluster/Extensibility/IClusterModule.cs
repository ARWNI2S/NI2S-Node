// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Cluster.Lifecycle;
using ARWNI2S.Extensibility;
using Orleans;

namespace ARWNI2S.Cluster.Extensibility
{
    public interface IClusterModule : IModule, IBuildModuleSupported, ILifecycleParticipant<IClusterNodeLifecycle>
    {
    }
}
