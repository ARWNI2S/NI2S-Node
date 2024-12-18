using Microsoft.Extensions.Caching.Memory;

namespace ARWNI2S.Engine.Caching
{
    /// <summary>
    /// Represents a local in-memory cache with distributed synchronization
    /// </summary>
    public interface ISynchronizedMemoryCache : IMemoryCache
    {
    }
}