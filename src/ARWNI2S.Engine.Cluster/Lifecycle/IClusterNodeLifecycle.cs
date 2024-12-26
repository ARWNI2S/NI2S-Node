using Orleans;

namespace ARWNI2S.Engine.Cluster.Lifecycle
{
    /// <summary>
    /// A <see cref="ILifecycleObservable"/> marker type for client lifecycles.
    /// </summary>
    public interface IClusterNodeLifecycle : ILifecycleObservable
    {
    }
}
