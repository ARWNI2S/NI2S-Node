namespace ARWNI2S.Engine.Runtime.Dispatcher
{
    internal class FrameTaskScheduler : TaskScheduler
    {
        public override int MaximumConcurrencyLevel => base.MaximumConcurrencyLevel;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            throw new NotImplementedException();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }
    }
}
