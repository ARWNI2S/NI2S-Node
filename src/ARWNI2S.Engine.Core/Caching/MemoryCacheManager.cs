﻿using ARWNI2S.Caching;
using ARWNI2S.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace ARWNI2S.Caching
{
    /// <summary>
    /// Represents a memory cache manager 
    /// </summary>
    /// <remarks>
    /// This class should be registered on IoC as singleton instance
    /// </remarks>
    public partial class MemoryCacheManager : CacheKeyService, IStaticCacheManager
    {
        #region Fields

        // Flag: Has Dispose already been called?
        private bool _disposed;

        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Holds the keys known by this dragonCorp instance
        /// </summary>
        private readonly ICacheKeyManager _keyManager;

        private static CancellationTokenSource _clearToken = new();

        #endregion

        #region Ctor

        public MemoryCacheManager(NI2SSettings ni2sSettings, IMemoryCache memoryCache, ICacheKeyManager cacheKeyManager)
            : base(ni2sSettings)
        {
            _memoryCache = memoryCache;
            _keyManager = cacheKeyManager;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare cache entry options for the passed key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cache entry options</returns>
        private MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
        {
            //set expiration time for the passed cache key
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(key.CacheTime)
            };

            //add token to clear cache entries
            options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
            options.RegisterPostEvictionCallback(OnEviction);
            _keyManager.AddKey(key.Key);

            return options;
        }

        private void OnEviction(object key, object value, EvictionReason reason, object state)
        {
            switch (reason)
            {
                // we clean up after ourselves elsewhere
                case EvictionReason.Removed:
                case EvictionReason.Replaced:
                case EvictionReason.TokenExpired:
                    break;
                // if the entry was evicted by the cache itself, we remove the key
                default:
                    _keyManager.RemoveKey(key as string);
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove the value with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            var key = PrepareKey(cacheKey, cacheKeyParameters).Key;
            _memoryCache.Remove(key);
            _keyManager.RemoveKey(key);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the cached value associated with the specified key
        /// </returns>
        public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
        {
            if ((key?.CacheTime ?? 0) <= 0)
                return await acquire();

            var value = _memoryCache.GetOrCreate(
                key.Key,
                entry =>
                {
                    entry.SetOptions(PrepareEntryOptions(key));
                    return new Lazy<Task<T>>(acquire, true);
                })?.Value;

            if (value != null)
                return await value;

            return default;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, return a default value
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="defaultValue">A default value to return if the key is not present in the cache</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the cached value associated with the specified key, or the default value if none was found
        /// </returns>
        public async Task<T> GetAsync<T>(CacheKey key, T defaultValue = default)
        {
            var value = _memoryCache.Get<Lazy<Task<T>>>(key.Key)?.Value;

            return value != null ? await value : defaultValue;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the cached value associated with the specified key
        /// </returns>
        public async Task<T> GetAsync<T>(CacheKey key, Func<T> acquire)
        {
            return await GetAsync(key, () => Task.FromResult(acquire()));
        }

        /// <summary>
        /// Add the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task SetAsync<T>(CacheKey key, T data)
        {
            if (data != null && (key?.CacheTime ?? 0) > 0)
                _memoryCache.Set(
                    key.Key,
                    new Lazy<Task<T>>(() => Task.FromResult(data), true),
                    PrepareEntryOptions(key));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Remove items by cache key prefix
        /// </summary>
        /// <param name="prefix">Cache key prefix</param>
        /// <param name="prefixParameters">Parameters to create cache key prefix</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
        {
            foreach (var key in _keyManager.RemoveByPrefix(PrepareKeyPrefix(prefix, prefixParameters)))
                _memoryCache.Remove(key);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task ClearAsync()
        {
            _clearToken.Cancel();
            _clearToken.Dispose();
            _clearToken = new CancellationTokenSource();
            _keyManager.Clear();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                // don't dispose of the MemoryCache, as it is injected
                _clearToken.Dispose();

            _disposed = true;
        }

        #endregion
    }
}
