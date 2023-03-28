
namespace NI2S.Node
{
    /// <summary>
    /// Both a lifecycle observer and observable lifecycle.
    /// </summary>
    public interface ILifecycleSubject : ILifecycleObservable, ILifecycleObserver
    {
    }
}
