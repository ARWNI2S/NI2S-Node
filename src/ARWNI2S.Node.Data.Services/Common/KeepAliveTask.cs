using ARWNI2S.Node.Data.Services.ScheduleTasks;

namespace ARWNI2S.Node.Data.Services.Common
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class KeepAliveTask : IScheduleTask
    {
        #region Fields

        private readonly ServerHttpClient _nodeHttpClient;

        #endregion

        #region Ctor

        public KeepAliveTask(ServerHttpClient serverHttpClient)
        {
            _nodeHttpClient = serverHttpClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public async Task ExecuteAsync()
        {
            await _nodeHttpClient.KeepAliveAsync();
        }

        #endregion
    }
}