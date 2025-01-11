namespace ARWNI2S.Engine.Threading.Tasks
{
    internal abstract class TaskFactoryBase : TaskFactory
    {
        protected TaskFactoryBase(CancellationToken token, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler taskScheduler)
            : base(token, creationOptions, continuationOptions, taskScheduler)
        {

        }
    }
}
