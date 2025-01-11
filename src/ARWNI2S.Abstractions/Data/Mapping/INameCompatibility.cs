// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Data.Mapping
{
    /// <summary>
    /// Backward compatibility of table naming
    /// </summary>
    public partial interface INameCompatibility
    {
        /// <summary>
        /// Gets table name for mapping with the type
        /// </summary>
        Dictionary<Type, string> TableNames { get; }

        /// <summary>
        ///  Gets column name for mapping with the entity's property and type
        /// </summary>
        Dictionary<(Type, string), string> ColumnName { get; }
    }
}