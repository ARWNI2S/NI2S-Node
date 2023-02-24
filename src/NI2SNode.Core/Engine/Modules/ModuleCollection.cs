using System.Collections;

namespace NI2S.Node.Engine.Modules
{
    internal class ModuleCollection : IModuleCollection
    {
        private static readonly KeyComparer ModuleKeyComparer = new();
        private readonly IModuleCollection? _defaults;
        private readonly int _initialCapacity;
        private IDictionary<Type, object>? _features;
        private volatile int _containerRevision;

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleCollection"/>.
        /// </summary>
        public ModuleCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleCollection"/> with the specified initial capacity.
        /// </summary>
        /// <param name="initialCapacity">The initial number of elements that the collection can contain.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="initialCapacity"/> is less than 0</exception>
        public ModuleCollection(int initialCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }

            _initialCapacity = initialCapacity;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleCollection"/> with the specified defaults.
        /// </summary>
        /// <param name="defaults">The feature defaults.</param>
        public ModuleCollection(IModuleCollection defaults)
        {
            _defaults = defaults;
        }

        /// <inheritdoc />
        public virtual int Revision
        {
            get { return _containerRevision + (_defaults?.Revision ?? 0); }
        }

        /// <inheritdoc />
        public bool IsReadOnly { get { return false; } }

        /// <inheritdoc />
        public object? this[Type key]
        {
            get
            {
                ArgumentNullException.ThrowIfNull(key, nameof(key));

                return _features != null && _features.TryGetValue(key, out var result) ? result : _defaults?[key];
            }
            set
            {
                ArgumentNullException.ThrowIfNull(key, nameof(key));

                if (value == null)
                {
                    if (_features != null && _features.Remove(key))
                    {
                        _containerRevision++;
                    }
                    return;
                }

                _features ??= new Dictionary<Type, object>(_initialCapacity);
                _features[key] = value;
                _containerRevision++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            if (_features != null)
            {
                foreach (var pair in _features)
                {
                    yield return pair;
                }
            }

            if (_defaults != null)
            {
                // Don't return features masked by the wrapper.
                foreach (var pair in _features == null ? _defaults : _defaults.Except(_features, ModuleKeyComparer))
                {
                    yield return pair;
                }
            }
        }

        /// <inheritdoc />
        public TModule? Get<TModule>()
        {
            return (TModule?)this[typeof(TModule)];
        }

        /// <inheritdoc />
        public void Set<TModule>(TModule? instance)
        {
            this[typeof(TModule)] = instance;
        }

        private sealed class KeyComparer : IEqualityComparer<KeyValuePair<Type, object>>
        {
            public bool Equals(KeyValuePair<Type, object> x, KeyValuePair<Type, object> y)
            {
                return x.Key.Equals(y.Key);
            }

            public int GetHashCode(KeyValuePair<Type, object> obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}
