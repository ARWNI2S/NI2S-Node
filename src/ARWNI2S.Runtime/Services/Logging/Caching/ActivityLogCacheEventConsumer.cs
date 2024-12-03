using ARWNI2S.Data.Entities.Logging;
using ARWNI2S.Services.Caching;

namespace ARWNI2S.Services.Logging.Caching
{
    /// <summary>
    /// Represents an activity log cache event consumer
    /// </summary>
    public partial class ActivityLogCacheEventConsumer : DataCacheEventConsumer<ActivityLog>
    {
    }
}