using ARWNI2S.Configuration;

namespace ARWNI2S.Caching;

/// <summary>
/// Represents a memory cache manager with distributed synchronization
/// </summary>
/// <remarks>
/// This class should be registered on IoC as singleton instance
/// </remarks>
public partial class SynchronizedMemoryCacheManager : MemoryCacheManager
{
    public SynchronizedMemoryCacheManager(NI2SSettings niisSettings,
        ISynchronizedMemoryCache memoryCache,
        ICacheKeyManager cacheKeyManager) : base(niisSettings, memoryCache, cacheKeyManager)
    {
    }
}