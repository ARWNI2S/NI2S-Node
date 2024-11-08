namespace ARWNI2S.Infrastructure.Lifecycle
{
    /// <summary>
    /// Both a lifecycle observer and observable lifecycle.
    /// </summary>
    public interface ILifecycleSubject : ILifecycleObservable, ILifecycleObserver
    {
    }
}
