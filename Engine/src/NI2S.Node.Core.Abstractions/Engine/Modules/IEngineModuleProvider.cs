// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System.Collections.Generic;

namespace NI2S.Node.Engine
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
        /// <param name="parts">The list of <see cref="NI2SPlugin"/> instances in the engine.
        /// </param>
        /// <param name="module">The module instance to populate.</param>
        /// <remarks>
        /// <see cref="NI2SPlugin"/> instances in <paramref name="parts"/> appear in the same ordered sequence they
        /// are stored in <see cref="NI2SModuleManager.NI2SPlugins"/>. This ordering may be used by the module
        /// provider to make precedence decisions.
        /// </remarks>
        void PopulateModule(IEnumerable<IEngineModule> modules, TModule module);
    }
}
