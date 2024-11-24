using ARWNI2S.Node.Core.Plugins;

namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Plugins uploaded notification
    /// </summary>
    public partial class PluginsUploaded
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="uploadedPlugins">Uploaded plugins</param>
        public PluginsUploaded(IList<PluginDescriptor> uploadedPlugins)
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