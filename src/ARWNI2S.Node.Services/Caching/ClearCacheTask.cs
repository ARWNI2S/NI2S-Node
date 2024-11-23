using ARWNI2S.Node.Caching;
using ARWNI2S.Node.Core.Scheduling;

namespace ARWNI2S.Node.Services.Caching
{
    /// <summary>
    /// Clear cache scheduled task implementation
    /// </summary>
    public partial class ClearCacheTask : IClusterJob
    {
        #region Fields

        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public ClearCacheTask(IStaticCacheManager staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public async Task ExecuteAsync()
        {
            await _staticCacheManager.ClearAsync();
        }

        #endregion
    }
}