// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Extensibility
{
    public interface IModuleCollection : IEnumerable<KeyValuePair<Type, IModule>>
    {
        /// <summary>
        /// Indicates if the collection can be modified.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Incremented for each modification and can be used to verify cached results.
        /// </summary>
        int Revision { get; }

        /// <summary>
        /// Gets or sets a given module. Setting a null value removes the module.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested module, or null if it is not present.</returns>
        IModule this[Type key] { get; set; }

        /// <summary>
        /// Retrieves the requested module from the collection.
        /// </summary>
        /// <typeparam name="TModule">The module key.</typeparam>
        /// <returns>The requested module, or null if it is not present.</returns>
        TModule Get<TModule>() where TModule : class, IModule;

        /// <summary>
        /// Sets the given module in the collection.
        /// </summary>
        /// <typeparam name="TModule">The module key.</typeparam>
        /// <param name="instance">The module value.</param>
        void Set<TModule>(TModule instance) where TModule : class, IModule;
    }
}