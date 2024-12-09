using ARWNI2S.Caching;
using ARWNI2S.Core.Configuration;

namespace ARWNI2S.Core.Caching
{
    /// <summary>
    /// Represents a memory cache manager with distributed synchronization
    /// </summary>
    /// <remarks>
    /// This class should be registered on IoC as singleton instance
    /// </remarks>
    public partial class SynchronizedMemoryCacheManager : MemoryCacheManager
    {
        public SynchronizedMemoryCacheManager(NI2SSettings ni2sSettings,
            ISynchronizedMemoryCache memoryCache,
            ICacheKeyManager cacheKeyManager) : base(ni2sSettings, memoryCache, cacheKeyManager)
        {
        }
    }
}