using ARWNI2S.Engine.Parts;

namespace ARWNI2S.Extensibility
{
    /// <summary>
    /// Marker interface for <see cref="IEngineModuleProvider"/>
    /// implementations.
    /// </summary>
    public interface IEngineModuleProvider
    {
    }

    /// <summary>
    /// A provider for a given <typeparamref name="TModule"/> module.
    /// </summary>
    /// <typeparam name="TModule">The type of the module.</typeparam>
    public interface IEngineModuleProvider<TModule> : IEngineModuleProvider
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
        void PopulateModule(IEnumerable<EnginePart> parts, TModule module);
    }
}