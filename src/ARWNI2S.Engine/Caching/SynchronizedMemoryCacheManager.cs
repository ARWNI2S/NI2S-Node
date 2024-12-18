using ARWNI2S.Engine.Configuration;

namespace ARWNI2S.Engine.Caching
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