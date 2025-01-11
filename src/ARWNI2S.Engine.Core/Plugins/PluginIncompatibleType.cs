// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Plugins
{
    public enum PluginIncompatibleType
    {
        /// <summary>
        /// The plugin isn't compatible with the current version
        /// </summary>
        NotCompatibleWithCurrentVersion,
        /// <summary>
        /// The main assembly isn't found
        /// </summary>
        MainAssemblyNotFound,
        /// <summary>
        /// Another version of the same library is already loaded in memory
        /// </summary>
        HasCollisions
    }
}
