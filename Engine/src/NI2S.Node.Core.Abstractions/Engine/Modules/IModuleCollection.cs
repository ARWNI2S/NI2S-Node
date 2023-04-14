// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;
using System.Collections.Generic;

namespace NI2S.Node.Engine
{
    /// <summary>
    /// Represents a collection of NI2S engine modules.
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
        /// Gets or sets a given feature. Setting a null value removes the feature.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested feature, or null if it is not present.</returns>
        object this[Type key] { get; set; }

        /// <summary>
        /// Retrieves the requested module from the collection.
        /// </summary>
        /// <typeparam name="TModule">The feature key.</typeparam>
        /// <returns>The requested feature, or null if it is not present.</returns>
        TModule Get<TModule>();

        /// <summary>
        /// Sets the given feature in the collection.
        /// </summary>
        /// <typeparam name="TModule">The feature key.</typeparam>
        /// <param name="instance">The feature value.</param>
        void Set<TModule>(TModule instance);
    }
}
