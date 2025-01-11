// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S
{
    internal interface IPureSingleton<T> : IDisposable
        where T : class
    {
        static T Instance { get; }
    }

    /// <summary>
    /// Provides access to all "singletons" stored by <see cref="Singleton{T}"/>.
    /// </summary>
    internal abstract class BaseSingleton
    {
        static BaseSingleton() => AllSingletons = new Dictionary<Type, object>();

        /// <summary>
        /// Dictionary of type to singleton instances.
        /// </summary>
        protected static IDictionary<Type, object> AllSingletons { get; }
    }

    /// <summary>
    /// A statically compiled "singleton" used to store objects throughout the 
    /// lifetime of the app domain. Not so much singleton in the pattern's 
    /// sense of the word as a standardized way to store single instances.
    /// </summary>
    /// <typeparam name="T">The type of object to store.</typeparam>
    /// <remarks>Access to the instance is not synchronized.</remarks>
    internal partial class Singleton<T> : BaseSingleton
    {
        private static T instance;

        /// <summary>
        /// The singleton instance for the specified type T. Only one instance (at the time) of this object for each type of T.
        /// </summary>
        public static T Instance
        {
            get => instance;
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }

    /// <summary>
    /// Provides a singleton list for a certain type.
    /// </summary>
    /// <typeparam name="T">The type of list to store.</typeparam>
    internal partial class SingletonList<T> : Singleton<IList<T>>
    {
        static SingletonList() => Singleton<IList<T>>.Instance = [];

        /// <summary>
        /// The singleton instance for the specified type T. Only one instance (at the time) of this list for each type of T.
        /// </summary>
        public static new IList<T> Instance => Singleton<IList<T>>.Instance;
    }

    /// <summary>
    /// Provides a singleton dictionary for a certain key and vlaue type.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    internal partial class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>> where TKey : notnull
    {
        static SingletonDictionary() => Singleton<Dictionary<TKey, TValue>>.Instance = [];

        /// <summary>
        /// The singleton instance for the specified type T. Only one instance (at the time) of this dictionary for each type of T.
        /// </summary>
        public static new IDictionary<TKey, TValue> Instance => Singleton<Dictionary<TKey, TValue>>.Instance;
    }
}
