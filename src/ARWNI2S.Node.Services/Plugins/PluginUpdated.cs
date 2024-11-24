using ARWNI2S.Node.Core.Plugins;

namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Represents the plugin updated notification
    /// </summary>
    public partial class PluginUpdated
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="plugin">Updated plugin</param>
        public PluginUpdated(PluginDescriptor plugin)
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