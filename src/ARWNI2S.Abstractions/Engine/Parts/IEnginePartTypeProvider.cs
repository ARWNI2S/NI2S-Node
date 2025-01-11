// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Reflection;

namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// Exposes a set of types from an <see cref="EnginePart"/>.
    /// </summary>
    public interface IEnginePartTypeProvider
    {
        /// <summary>
        /// Gets the list of available types in the <see cref="EnginePart"/>.
        /// </summary>
        IEnumerable<TypeInfo> Types { get; }
    }
}