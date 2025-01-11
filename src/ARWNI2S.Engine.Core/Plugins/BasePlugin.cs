// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Plugins
{
    /// <summary>
    /// Base plugin
    /// </summary>
    public abstract partial class BasePlugin : IPlugin
    {
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public virtual string GetConfigurationPageUrl()
        {
            return null;
        }

        /// <summary>
        /// Gets or sets the plugin descriptor
        /// </summary>
        public virtual PluginDescriptor PluginDescriptor { get; set; }

        /// <summary>
        /// Install plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual Task InstallAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual Task UninstallAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Update plugin
        /// </summary>
        /// <param name="currentVersion">Current version of plugin</param>
        /// <param name="targetVersion">New version of plugin</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual Task UpdateAsync(string currentVersion, string targetVersion)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Prepare plugin to the uninstallation
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual Task PreparePluginToUninstallAsync()
        {
            //any can put any custom validation logic here
            //throw an exception if this plugin cannot be uninstalled
            //for example, requires some other certain plugins to be uninstalled first
            return Task.CompletedTask;
        }
    }
}
