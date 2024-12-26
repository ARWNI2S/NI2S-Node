﻿using ARWNI2S.Extensibility;
using System.Collections;

namespace ARWNI2S.Engine.Builder
{
    /// <summary>
    /// Default implementation for <see cref="IModuleCollection"/>.
    /// </summary>
    public class ModuleCollection : IModuleCollection
    {
        private static readonly KeyComparer ModuleKeyComparer = new KeyComparer();
        private readonly IModuleCollection _defaults;
        private readonly int _initialCapacity;
        private IDictionary<Type, IEngineModule> _modules;
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
            ArgumentOutOfRangeException.ThrowIfNegative(initialCapacity);

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
        public IEngineModule this[Type key]
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
                    _modules = new Dictionary<Type, IEngineModule>(_initialCapacity);
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
        public IEnumerator<KeyValuePair<Type, IEngineModule>> GetEnumerator()
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
                // Don't return features masked by the wrapper.
                foreach (var pair in _modules == null ? _defaults : _defaults.Except(_modules, ModuleKeyComparer))
                {
                    yield return pair;
                }
            }
        }

        /// <inheritdoc />
        public TModule Get<TModule>() where TModule : class,  IEngineModule
        {
            if (typeof(TModule).IsValueType)
            {
                var feature = this[typeof(TModule)];
                if (feature is null && Nullable.GetUnderlyingType(typeof(TModule)) is null)
                {
                    throw new InvalidOperationException(
                        $"{typeof(TModule).FullName} does not exist in the feature collection " +
                        $"and because it is a struct the method can't return null. Use 'featureCollection[typeof({typeof(TModule).FullName})] is not null' to check if the feature exists.");
                }
                return (TModule)feature;
            }
            return (TModule)this[typeof(TModule)];
        }

        /// <inheritdoc />
        public void Set<TModule>(TModule instance) where TModule : class,  IEngineModule
        {
            this[typeof(TModule)] = instance;
        }

        // Used by the debugger. Count over enumerable is required to get the correct value.
        private int GetCount() => this.Count();

        private sealed class KeyComparer : IEqualityComparer<KeyValuePair<Type, IEngineModule>>
        {
            public bool Equals(KeyValuePair<Type, IEngineModule> x, KeyValuePair<Type, IEngineModule> y)
            {
                return x.Key.Equals(y.Key);
            }

            public int GetHashCode(KeyValuePair<Type, IEngineModule> obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}