using ARWNI2S.Node.Infrastructure;
using System.Collections;

namespace ARWNI2S.Node.Engine
{
    /// <summary>
    /// Default implementation for <see cref="IEngineModules"/>.
    /// </summary>
    public class EngineModules : IEngineModules
    {
        private static readonly KeyComparer ModuleKeyComparer = new();
        private readonly IEngineModules _defaults;
        private readonly int _initialCapacity;
        private IDictionary<Type, object> _modules;
        private volatile int _containerRevision;

        /// <summary>
        /// Initializes a new instance of <see cref="EngineModules"/>.
        /// </summary>
        public EngineModules()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EngineModules"/> with the specified initial capacity.
        /// </summary>
        /// <param name="initialCapacity">The initial number of elements that the collection can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="initialCapacity"/> is less than 0</exception>
        public EngineModules(int initialCapacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(initialCapacity);

            _initialCapacity = initialCapacity;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EngineModules"/> with the specified defaults.
        /// </summary>
        /// <param name="defaults">The module defaults.</param>
        public EngineModules(IEngineModules defaults)
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
        public object this[Type key]
        {
            get
            {
                ArgumentNullException.ThrowIfNull(key);

                return _modules != null && _modules.TryGetValue(key, out var result) ? result : _defaults?[key];
            }
            set
            {
                ArgumentNullException.ThrowIfNull(key);

                if (value == null)
                {
                    if (_modules != null && _modules.Remove(key))
                    {
                        _containerRevision++;
                    }
                    return;
                }

                if (_modules == null)
                {
                    _modules = new Dictionary<Type, object>(_initialCapacity);
                }
                _modules[key] = value;
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
            if (_modules != null)
            {
                foreach (var pair in _modules)
                {
                    yield return pair;
                }
            }

            if (_defaults != null)
            {
                // Don't return modules masked by the wrapper.
                foreach (var pair in _modules == null ? _defaults : _defaults.Except(_modules, ModuleKeyComparer))
                {
                    yield return pair;
                }
            }
        }

        /// <inheritdoc />
        public TModule Get<TModule>()
        {
            if (typeof(TModule).IsValueType)
            {
                var module = this[typeof(TModule)];
                if (module is null && Nullable.GetUnderlyingType(typeof(TModule)) is null)
                {
                    throw new InvalidOperationException(
                        $"{typeof(TModule).FullName} does not exist in the module collection " +
                        $"and because it is a struct the method can't return null. Use 'moduleCollection[typeof({typeof(TModule).FullName})] is not null' to check if the module exists.");
                }
                return (TModule)module;
            }
            return (TModule)this[typeof(TModule)];
        }

        /// <inheritdoc />
        public void Set<TModule>(TModule instance)
        {
            this[typeof(TModule)] = instance;
        }

        // Used by the debugger. Count over enumerable is required to get the correct value.
        private int GetCount() => this.Count();

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
