// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// A part of an NI2S application.
    /// </summary>
    public abstract class EnginePart
    {
        /// <summary>
        /// Gets the <see cref="EnginePart"/> name.
        /// </summary>
        public abstract string Name { get; }
    }
}