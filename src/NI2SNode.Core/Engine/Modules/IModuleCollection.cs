namespace NI2S.Node.Engine.Modules
{
    /// <summary>
    /// Represents a collection of engine modules.
    /// </summary>
    public interface IModuleCollection : IEnumerable<KeyValuePair<Type, object>>
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
        /// <param name="key">The module key.</param>
        /// <returns>The requested module, or null if it is not present.</returns>
        object? this[Type key] { get; set; }

        /// <summary>
        /// Retrieves the requested module from the collection.
        /// </summary>
        /// <typeparam name="TModule">The module key.</typeparam>
        /// <returns>The requested module, or null if it is not present.</returns>
        TModule? Get<TModule>();

        /// <summary>
        /// Sets the given module in the collection.
        /// </summary>
        /// <typeparam name="TModule">The module key.</typeparam>
        /// <param name="instance">The module value.</param>
        void Set<TModule>(TModule? instance);
    }

}
