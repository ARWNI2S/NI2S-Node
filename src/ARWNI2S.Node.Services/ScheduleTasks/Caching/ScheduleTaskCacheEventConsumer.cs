using ARWNI2S.Node.Core.Entities.ScheduleTasks;
using ARWNI2S.Node.Services.Caching;

namespace ARWNI2S.Node.Services.ScheduleTasks.Caching
{
    /// <summary>
    /// Represents a schedule task cache event consumer
    /// </summary>
    public partial class ScheduleTaskCacheEventConsumer : CacheEventConsumer<ScheduleTask>
    {
    }
}
