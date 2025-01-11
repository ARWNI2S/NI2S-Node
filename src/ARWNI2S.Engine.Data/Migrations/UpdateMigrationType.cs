// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Data.Migrations
{
    /// <summary>
    /// Represents an update migration type
    /// </summary>
    public enum UpdateMigrationType
    {
        /// <summary>
        /// Database data
        /// </summary>
        Data = 5,

        /// <summary>
        /// Localization
        /// </summary>
        Localization = 10,

        /// <summary>
        /// Setting
        /// </summary>
        Settings = 15
    }
}