// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Caching
{
    /// <summary>
    /// Cache key service interface
    /// </summary>
    public partial interface ICacheKeyService
    {
        /// <summary>
        /// Create a copy of cache key and fills it by passed parameters
        /// </summary>
        /// <param name="cacheKey">Initial cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        /// <returns>Cache key</returns>
        CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters);

        /// <summary>
        /// Create a copy of cache key using the default cache time and fills it by passed parameters
        /// </summary>
        /// <param name="cacheKey">Initial cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        /// <returns>Cache key</returns>
        CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters);
    }
}