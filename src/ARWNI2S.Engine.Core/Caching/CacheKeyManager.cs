// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Caching;
using ARWNI2S.Collections;

namespace ARWNI2S.Engine.Caching
{
    /// <summary>
    /// Cache key manager
    /// </summary>
    /// <remarks>
    /// This class should be registered on IoC as singleton instance
    /// </remarks>
    public partial class CacheKeyManager : ICacheKeyManager
    {
        protected readonly IConcurrentCollection<byte> _keys;

        public CacheKeyManager(IConcurrentCollection<byte> keys)
        {
            _keys = keys;
        }

        /// <summary>
        /// Add the key
        /// </summary>
        /// <param name="key">The key to add</param>
        public void AddKey(string key)
        {
            _keys.Add(key, default);
        }

        /// <summary>
        /// Remove the key
        /// </summary>
        /// <param name="key">The key to remove</param>
        public void RemoveKey(string key)
        {
            _keys.Remove(key);
        }

        /// <summary>
        /// Remove all keys
        /// </summary>
        public void Clear()
        {
            _keys.Clear();
        }

        /// <summary>
        /// Remove keys by prefix
        /// </summary>
        /// <param name="prefix">Prefix to delete keys</param>
        /// <returns>The list of removed keys</returns>
        public IEnumerable<string> RemoveByPrefix(string prefix)
        {
            return _keys.Prune(prefix, out var subtree)
                ? subtree?.Keys
                : [];
        }

        /// <summary>
        /// The list of keys
        /// </summary>
        public IEnumerable<string> Keys => _keys.Keys;
    }
}
