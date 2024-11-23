using ARWNI2S.Node.Configuration;

namespace ARWNI2S.Node.Caching;

/// <summary>
/// Represents a memory cache manager with distributed synchronization
/// </summary>
/// <remarks>
/// This class should be registered on IoC as singleton instance
/// </remarks>
public partial class SynchronizedMemoryCacheManager : MemoryCacheManager
{
    public SynchronizedMemoryCacheManager(NiisSettings niisSettings,
        ISynchronizedMemoryCache memoryCache,
        ICacheKeyManager cacheKeyManager) : base(niisSettings, memoryCache, cacheKeyManager)
    {
    }
}