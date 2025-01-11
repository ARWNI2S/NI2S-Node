// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Microsoft.Extensions.Caching.Memory;

namespace ARWNI2S.Caching
{
    /// <summary>
    /// Represents a local in-memory cache with distributed synchronization
    /// </summary>
    public interface ISynchronizedMemoryCache : IMemoryCache
    {
    }
}