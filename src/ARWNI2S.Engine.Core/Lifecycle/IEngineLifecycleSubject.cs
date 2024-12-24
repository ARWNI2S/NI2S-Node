using Orleans;

namespace ARWNI2S.Engine.Lifecycle
{
    /// <summary>
    /// Observable silo lifecycle and observer.
    /// </summary>
    public interface IEngineLifecycleSubject : IEngineLifecycle, ILifecycleObserver
    {
    }
}
