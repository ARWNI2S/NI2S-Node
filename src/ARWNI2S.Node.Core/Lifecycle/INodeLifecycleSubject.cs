using ARWNI2S.Infrastructure.Lifecycle;

namespace ARWNI2S.Node.Core.Lifecycle
{
    /// <summary>
    /// Observable silo lifecycle and observer.
    /// </summary>
    public interface INodeLifecycleSubject : INodeLifecycle, ILifecycleObserver
    {
    }
}
