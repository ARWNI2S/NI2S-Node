// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Plugins
{
    /// <summary>
    /// Represents a mode to load plugins
    /// </summary>
    public enum LoadPluginsMode
    {
        /// <summary>
        /// All (Installed and Not installed)
        /// </summary>
        All = 0,

        /// <summary>
        /// Installed only
        /// </summary>
        InstalledOnly = 10,

        /// <summary>
        /// Not installed only
        /// </summary>
        NotInstalledOnly = 20
    }
}