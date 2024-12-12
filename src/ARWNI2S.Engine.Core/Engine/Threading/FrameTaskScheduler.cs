namespace ARWNI2S.Engine.Threading
{
    internal class FrameTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return null;
        }

        protected override void QueueTask(Task task)
        {

        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }
    }
}
