﻿using ARWNI2S.Infrastructure.Entities;

namespace ARWNI2S.Node.Services.Plugins
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
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of plugins
        /// </returns>
        Task<IList<TPlugin>> LoadAllPluginsAsync(IUser user = null, int nodeId = 0);

        /// <summary>
        /// Load plugin by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the plugin
        /// </returns>
        Task<TPlugin> LoadPluginBySystemNameAsync(string systemName, IUser user = null, int nodeId = 0);

        /// <summary>
        /// Load active plugins
        /// </summary>
        /// <param name="systemNames">System names of active plugins</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of active plugins
        /// </returns>
        Task<IList<TPlugin>> LoadActivePluginsAsync(List<string> systemNames, IUser user = null, int nodeId = 0);

        /// <summary>
        /// Check whether the passed plugin is active
        /// </summary>
        /// <param name="plugin">Plugin to check</param>
        /// <param name="systemNames">System names of active plugins</param>
        /// <returns>Result</returns>
        bool IsPluginActive(TPlugin plugin, List<string> systemNames);

        ///// <summary>
        ///// Get plugin logo URL
        ///// </summary>
        ///// <param name="plugin">Plugin</param>
        ///// <returns>
        ///// A task that represents the asynchronous operation
        ///// The task result contains the logo URL
        ///// </returns>
        //Task<string> GetPluginLogoUrlAsync(TPlugin plugin);
    }
}