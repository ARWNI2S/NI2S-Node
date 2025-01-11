// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Parts;

namespace ARWNI2S.Extensibility
{
    /// <summary>
    /// Marker interface for <see cref="IEngineServiceProvider"/>
    /// implementations.
    /// </summary>
    public interface IEngineServiceProvider
    {
    }

    /// <summary>
    /// A provider for a given <typeparamref name="TService"/> service.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    public interface IEngineServiceProvider<TService> : IEngineServiceProvider
    {
        /// <summary>
        /// Updates the <paramref name="module"/> instance.
        /// </summary>
        /// <param name="parts">The list of <see cref="EnginePart"/> instances in the engine.
        /// </param>
        /// <param name="module">The module instance to populate.</param>
        /// <remarks>
        /// <see cref="EnginePart"/> instances in <paramref name="parts"/> appear in the same ordered sequence they
        /// are stored in <see cref="IEnginePartManager.EngineParts"/>. This ordering may be used by the module
        /// provider to make precedence decisions.
        /// </remarks>
        void PopulateModule(IEnumerable<EnginePart> parts, TService module);
    }
}