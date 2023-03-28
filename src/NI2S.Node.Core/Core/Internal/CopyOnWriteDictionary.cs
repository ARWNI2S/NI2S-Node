using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node.Core.Internal
{
    internal sealed class CopyOnWriteDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
    {
        private readonly IDictionary<TKey, TValue> _sourceDictionary;
        private readonly IEqualityComparer<TKey> _comparer;
        private IDictionary<TKey, TValue> _innerDictionary;

        public CopyOnWriteDictionary(
            IDictionary<TKey, TValue> sourceDictionary,
            IEqualityComparer<TKey> comparer)
        {
            ArgumentNullException.ThrowIfNull(sourceDictionary);
            ArgumentNullException.ThrowIfNull(comparer);

            _sourceDictionary = sourceDictionary;
            _comparer = comparer;
        }

        private IDictionary<TKey, TValue> ReadDictionary
        {
            get
            {
                return _innerDictionary ?? _sourceDictionary;
            }
        }

        private IDictionary<TKey, TValue> WriteDictionary
        {
            get
            {
                _innerDictionary ??= new Dictionary<TKey, TValue>(_sourceDictionary, _comparer);

                return _innerDictionary;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return ReadDictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return ReadDictionary.Values;
            }
        }

        public int Count
        {
            get
            {
                return ReadDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return ReadDictionary[key];
            }
            set
            {
                WriteDictionary[key] = value;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return ReadDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            WriteDictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return WriteDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return ReadDictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            WriteDictionary.Add(item);
        }

        public void Clear()
        {
            WriteDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ReadDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ReadDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return WriteDictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ReadDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
