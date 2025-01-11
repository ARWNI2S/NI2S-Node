// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Plugins
{
    /// <summary>
    /// Plugins uploaded event
    /// </summary>
    public partial class PluginsUploadedEvent
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="uploadedPlugins">Uploaded plugins</param>
        public PluginsUploadedEvent(IList<PluginDescriptor> uploadedPlugins)
        {
            UploadedPlugins = uploadedPlugins;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Uploaded plugins
        /// </summary>
        public IList<PluginDescriptor> UploadedPlugins { get; }

        #endregion
    }
}