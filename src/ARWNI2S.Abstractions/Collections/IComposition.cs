namespace ARWNI2S.Collections
{
    public interface IComposition<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
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
        /// Gets or sets a given component. Setting a null value removes the component.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested component, or null if it is not present.</returns>
        TValue this[TKey key] { get; set; }

        /// <summary>
        /// Retrieves the requested component from the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TComponent">The component key.</typeparam>
        /// <returns>The requested component, or null if it is not present.</returns>
        TComponent Get<TComponent>(TKey key) where TComponent : class, TValue;

        /// <summary>
        /// Sets the given component in the collection.
        /// </summary>
        /// <typeparam name="TComponent">The component key.</typeparam>
        /// <param name="component">The component value.</param>
        void Set<TComponent>(TComponent component) where TComponent : class, TValue;
    }
}