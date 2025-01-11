// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Plugins
{
    /// <summary>
    /// Represents the plugin updated event
    /// </summary>
    public partial class PluginUpdatedEvent
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="plugin">Updated plugin</param>
        public PluginUpdatedEvent(PluginDescriptor plugin)
        {
            Plugin = plugin;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Updated plugin
        /// </summary>
        public PluginDescriptor Plugin { get; }

        #endregion
    }
}