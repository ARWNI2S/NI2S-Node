// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Environment.Mapper
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public interface IOrderedMapperProfile
    {
        /// <summary>
        /// Gets order of this configuration implementation
        /// </summary>
        int Order { get; }
    }
}
