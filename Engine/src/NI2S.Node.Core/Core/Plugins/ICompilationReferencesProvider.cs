// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using System.Collections.Generic;

namespace NI2S.Node.Core.Plugins
{
    /// <summary>
    /// Exposes one or more reference paths from an <see cref="NI2SPlugin"/>.
    /// </summary>
    public interface ICompilationReferencesProvider
    {
        /// <summary>
        /// Gets reference paths used to perform runtime compilation.
        /// </summary>
        IEnumerable<string> GetReferencePaths();
    }
}