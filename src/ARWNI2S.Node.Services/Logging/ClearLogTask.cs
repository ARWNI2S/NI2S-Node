using ARWNI2S.Node.Data.Entities.Common;
using ARWNI2S.Node.Data.Services.ScheduleTasks;

namespace ARWNI2S.Node.Data.Services.Logging
{
    /// <summary>
    /// Represents a task to clear [Log] table
    /// </summary>
    public partial class ClearLogTask : IScheduleTask
    {
        #region Fields

        private readonly CommonSettings _commonSettings;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public ClearLogTask(CommonSettings commonSettings,
            ILogger logger)
        {
            _commonSettings = commonSettings;
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public virtual async Task ExecuteAsync()
        {
            var utcNow = DateTime.UtcNow;

            await _logger.ClearLogAsync(_commonSettings.ClearLogOlderThanDays == 0 ? null : utcNow.AddDays(-_commonSettings.ClearLogOlderThanDays));
        }

        #endregion
    }
}