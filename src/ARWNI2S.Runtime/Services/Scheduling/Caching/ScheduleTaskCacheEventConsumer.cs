using ARWNI2S.Data.Entities.Scheduling;
using ARWNI2S.Services.Caching;

namespace ARWNI2S.Services.Scheduling.Caching
{
    /// <summary>
    /// Represents a schedule task cache event consumer
    /// </summary>
    public partial class ScheduleTaskCacheEventConsumer : DataCacheEventConsumer<ScheduleTask>
    {
    }
}
