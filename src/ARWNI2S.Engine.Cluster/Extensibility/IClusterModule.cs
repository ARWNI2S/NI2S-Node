using ARWNI2S.Cluster.Lifecycle;
using ARWNI2S.Extensibility;
using Orleans;

namespace ARWNI2S.Cluster
{
    public interface IClusterModule : IModule, ILifecycleParticipant<IClusterNodeLifecycle>
    {
    }
}
