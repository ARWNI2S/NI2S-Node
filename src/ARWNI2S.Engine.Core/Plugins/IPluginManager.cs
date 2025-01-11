// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Plugins
{
    /// <summary>
    /// Represents a plugin manager
    /// </summary>
    /// <typeparam name="TPlugin">Type of plugin</typeparam>
    public partial interface IPluginManager<TPlugin> where TPlugin : class, IPlugin
    {
        /// <summary>
        /// Load all plugins
        /// </summary>
        /// <param name="nodeId">Filter by node; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of plugins
        /// </returns>
        Task<IList<TPlugin>> LoadAllPluginsAsync(int nodeId = 0);

        /// <summary>
        /// Load plugin by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the plugin
        /// </returns>
        Task<TPlugin> LoadPluginBySystemNameAsync(string systemName, int nodeId = 0);

        /// <summary>
        /// Load active plugins
        /// </summary>
        /// <param name="systemNames">System names of active plugins</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of active plugins
        /// </returns>
        Task<IList<TPlugin>> LoadActivePluginsAsync(List<string> systemNames, int nodeId = 0);

        /// <summary>
        /// Check whether the passed plugin is active
        /// </summary>
        /// <param name="plugin">Plugin to check</param>
        /// <param name="systemNames">System names of active plugins</param>
        /// <returns>Result</returns>
        bool IsPluginActive(TPlugin plugin, List<string> systemNames);
    }
}