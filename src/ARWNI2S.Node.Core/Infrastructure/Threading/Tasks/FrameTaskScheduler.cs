
namespace ARWNI2S.Node.Infrastructure.Threading.Tasks
{
    internal class FrameTaskScheduler : TaskScheduler
    {
        public override int MaximumConcurrencyLevel => Environment.ProcessorCount;

        public FrameTaskScheduler() : base() { }

        protected override bool TryDequeue(Task task)
        {
            return base.TryDequeue(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {

        }

        protected override void QueueTask(Task task)
        {

        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {

        }
    }
}
