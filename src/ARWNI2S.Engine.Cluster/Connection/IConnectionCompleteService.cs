namespace ARWNI2S.Cluster.Connection
{
    internal interface IConnectionCompleteService
    {
        void OnCompleted(Func<object, Task> callback, object state);
    }
}