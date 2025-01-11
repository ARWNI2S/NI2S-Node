// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Caching;
using ARWNI2S.Configuration;

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
        public SynchronizedMemoryCacheManager(NI2SSettings nodeSettings,
            ISynchronizedMemoryCache memoryCache,
            ICacheKeyManager cacheKeyManager) : base(nodeSettings, memoryCache, cacheKeyManager)
        {
        }
    }
}