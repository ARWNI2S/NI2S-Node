using System.Reflection;

namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// Exposes a set of types from an <see cref="IEnginePart"/>.
    /// </summary>
    public interface IEnginePartTypeProvider
    {
        /// <summary>
        /// Gets the list of available types in the <see cref="IEnginePart"/>.
        /// </summary>
        IEnumerable<TypeInfo> Types { get; }
    }
}
