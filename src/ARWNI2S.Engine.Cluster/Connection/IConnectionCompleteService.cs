
namespace ARWNI2S.Engine.Cluster.Connection
{
    internal interface IConnectionCompleteService
    {
        void OnCompleted(Func<object, Task> callback, object state);
    }
}