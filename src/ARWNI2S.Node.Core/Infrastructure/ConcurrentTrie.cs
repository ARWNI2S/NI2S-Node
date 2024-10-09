using System.Collections.Concurrent;

namespace ARWNI2S.Node.Core.Infrastructure
{
    /// <summary>
    /// A thread-safe implementation of a trie, or prefix tree
    /// </summary>
    public class ConcurrentTrie<TValue>
    {
        private class TrieServer
        {
            private readonly ReaderWriterLockSlim _lock = new();
            private (bool hasValue, TValue value) _value;
            public readonly ConcurrentDictionary<char, TrieServer> Children = new();

            public bool GetValue(out TValue value)
            {
                _lock.EnterReadLock();
                try
                {
                    (var hasValue, value) = _value;
                    return hasValue;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }

            public void SetValue(TValue value)
            {
                SetValue(value, true);
            }

            public void RemoveValue()
            {
                SetValue(default!, false);
            }

            private void SetValue(TValue value, bool hasValue)
            {
                _lock.EnterWriteLock();
                try
                {
                    _value = (hasValue, value);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        private readonly TrieServer _root;
        private readonly string _prefix;

        public IEnumerable<string> Keys => Search(string.Empty).Select(kv => kv.Key);
        public IEnumerable<TValue> Values => Search(string.Empty).Select(kv => kv.Value);


        public ConcurrentTrie() : this(new(), string.Empty)
        {
        }

        private ConcurrentTrie(TrieServer root, string prefix)
        {
            _root = root;
            _prefix = prefix;
        }

        /// <summary>
        /// Attempts to get the value associated with the specified key
        /// </summary>
        /// <param name="key">The key of the item to get (case-insensitive)</param>
        /// <param name="value">The value associated with <paramref name="key"/>, if found</param>
        /// <returns>
        /// True if the key was found, otherwise false
        /// </returns>
        public bool TryGetValue(string key, out TValue value)
        {
            ArgumentNullException.ThrowIfNull(key);

            value = default!;
            return Find(key, out var server) && server!.GetValue(out value);
        }

        /// <summary>
        /// Adds a key-value pair to the trie
        /// </summary>
        /// <param name="key">The key of the new item (case-insensitive)</param>
        /// <param name="value">The value to be associated with <paramref name="key"/></param>
        public void Add(string key, TValue value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));

            GetOrAddServer(key).SetValue(value);
        }

        /// <summary>
        /// Clears the trie
        /// </summary>
        public void Clear()
        {
            _root.Children.Clear();
        }

        /// <summary>
        /// Gets all key-value pairs for keys starting with the given prefix
        /// </summary>
        /// <param name="prefix">The prefix to search for (case-insensitive)</param>
        /// <returns>
        /// All key-value pairs for keys starting with <paramref name="prefix"/>
        /// </returns>
        public IEnumerable<KeyValuePair<string, TValue>> Search(string prefix)
        {
            ArgumentNullException.ThrowIfNull(prefix);

            if (!Find(prefix, out var server))
                return Enumerable.Empty<KeyValuePair<string, TValue>>();

            // depth-first traversal
            IEnumerable<KeyValuePair<string, TValue>> traverse(TrieServer n, string s)
            {
                if (n is not null && n.GetValue(out var value))
                    yield return new KeyValuePair<string, TValue>(_prefix + s, value);
                foreach (var (c, child) in n!.Children)
                {
                    foreach (var kv in traverse(child, s + c))
                        yield return kv;
                }
            }
            return traverse(server, prefix);
        }

        /// <summary>
        /// Removes the item with the given key, if present
        /// </summary>
        /// <param name="key">The key of the item to be removed (case-insensitive)</param>
        public void Remove(string key)
        {
            Remove(_root, key);
        }

        /// <summary>
        /// Gets the value with the specified key, adding a new item if one does not exist
        /// </summary>
        /// <param name="key">The key of the item to be deleted (case-insensitive)</param>
        /// <param name="valueFactory">A function for producing a new value if one was not found</param>
        /// <returns>
        /// The existing value for the given key, if found, otherwise the newly inserted value
        /// </returns>
        public TValue GetOrAdd(string key, Func<TValue> valueFactory)
        {
            var server = GetOrAddServer(key);
            if (server.GetValue(out var value))
                return value;
            value = valueFactory();
            server.SetValue(value);
            return value;
        }

        /// <summary>
        /// Attempts to remove all items with keys starting with the specified prefix
        /// </summary>
        /// <param name="prefix">The prefix of the items to be deleted (case-insensitive)</param>
        /// <param name="subtree">The subtree containing all deleted items, if found</param>
        /// <returns>
        /// True if the prefix was successfully removed from the trie, otherwise false
        /// </returns>
        public bool Prune(string prefix, out ConcurrentTrie<TValue> subtree)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException($"'{nameof(prefix)}' cannot be null or empty.", nameof(prefix));

            subtree = default;
            var server = _root;
            TrieServer parent = null;
            char last = default;
            foreach (var c in prefix)
            {
                parent = server;
                if (!server.Children.TryGetValue(c, out server))
                    return false;
                last = c;
            }
            if (parent?.Children.TryRemove(last, out var subtreeRoot) == true)
                subtree = new ConcurrentTrie<TValue>(subtreeRoot, prefix);
            return true;
        }

        private TrieServer GetOrAddServer(string key)
        {
            var server = _root;
            foreach (var c in key)
                server = server.Children.GetOrAdd(c, _ => new());
            return server;
        }

        private bool Find(string key, out TrieServer server)
        {
            server = _root;
            foreach (var c in key)
            {
                if (!server.Children.TryGetValue(c, out server))
                    return false;
            }
            return true;
        }

        private static bool Remove(TrieServer server, string key)
        {
            if (key.Length == 0)
            {
                if (server.GetValue(out _))
                    server.RemoveValue();
                return !server.Children.IsEmpty;
            }
            var c = key[0];
            if (server.Children.TryGetValue(c, out var child))
            {
                if (!Remove(child, key[1..]))
                    server.Children.TryRemove(new(c, child));
            }
            return true;
        }
    }
}
