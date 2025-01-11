// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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
