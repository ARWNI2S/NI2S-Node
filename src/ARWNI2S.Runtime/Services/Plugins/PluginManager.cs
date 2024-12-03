using ARWNI2S.Data.Entities.Users;
using ARWNI2S.Plugins;
using ARWNI2S.Services.Users;

namespace ARWNI2S.Services.Plugins
{
    /// <summary>
    /// Represents a plugin manager implementation
    /// </summary>
    /// <typeparam name="TPlugin">Type of plugin</typeparam>
    public partial class PluginManager<TPlugin> : IPluginManager<TPlugin> where TPlugin : class, IPlugin
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IPluginService _pluginService;

        private readonly Dictionary<string, IList<TPlugin>> _plugins = [];

        #endregion

        #region Ctor

        public PluginManager(IUserService userService,
            IPluginService pluginService)
        {
            _userService = userService;
            _pluginService = pluginService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare the dictionary key to node loaded plugins
        /// </summary>
        /// <param name="user">INI2SUser</param>
        /// <param name="nodeId">Node identifier</param>
        /// <param name="systemName">Plugin system name</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the key
        /// </returns>
        protected virtual async Task<string> GetKeyAsync(User user, int nodeId, string systemName = null)
        {
            return $"{nodeId}-{(user != null ? string.Join(',', await _userService.GetUserRoleIdsAsync((User)user)) : null)}-{systemName}";
        }

        /// <summary>
        /// Load primary active plugin
        /// </summary>
        /// <param name="systemName">System name of primary active plugin</param>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the plugin
        /// </returns>
        protected virtual async Task<TPlugin> LoadPrimaryPluginAsync(string systemName, User user = null, int nodeId = 0)
        {
            //try to get a plugin by system name or return the first loaded one (it's necessary to have a primary active plugin)
            var plugin = await LoadPluginBySystemNameAsync(systemName, user, nodeId)
                         ?? (await LoadAllPluginsAsync(user, nodeId)).FirstOrDefault();

            return plugin;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load all plugins
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of plugins
        /// </returns>
        public virtual async Task<IList<TPlugin>> LoadAllPluginsAsync(User user = null, int nodeId = 0)
        {
            //get plugins and put them into the dictionary to avoid further loading
            var key = await GetKeyAsync(user, nodeId);

            if (!_plugins.TryGetValue(key, out var _))
                _plugins.Add(key, await _pluginService.GetPluginsAsync<TPlugin>(user: user, nodeId: nodeId));

            return _plugins[key];
        }

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
        public virtual async Task<TPlugin> LoadPluginBySystemNameAsync(string systemName, User user = null, int nodeId = 0)
        {
            if (string.IsNullOrEmpty(systemName))
                return null;

            //try to get already loaded plugin
            var key = await GetKeyAsync(user, nodeId, systemName);
            if (_plugins.TryGetValue(key, out var plugins1))
                return plugins1.FirstOrDefault();

            //or get it from list of all loaded plugins or load it for the first time
            var pluginBySystemName = _plugins.TryGetValue(await GetKeyAsync(user, nodeId), out var plugins2)
                && plugins2.FirstOrDefault(plugin =>
                    plugin.PluginDescriptor.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) is TPlugin loadedPlugin
                ? loadedPlugin
                : (await _pluginService.GetPluginDescriptorBySystemNameAsync<TPlugin>(systemName, user: user, nodeId: nodeId))?.Instance<TPlugin>();

            _plugins.Add(key, [pluginBySystemName]);

            return pluginBySystemName;
        }

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
        public virtual async Task<IList<TPlugin>> LoadActivePluginsAsync(List<string> systemNames, User user = null, int nodeId = 0)
        {
            if (systemNames == null)
                return [];

            //get loaded plugins according to passed system names
            return (await LoadAllPluginsAsync(user, nodeId))
                .Where(plugin => systemNames.Contains(plugin.PluginDescriptor.SystemName, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Check whether the passed plugin is active
        /// </summary>
        /// <param name="plugin">Plugin to check</param>
        /// <param name="systemNames">System names of active plugins</param>
        /// <returns>Result</returns>
        public virtual bool IsPluginActive(TPlugin plugin, List<string> systemNames)
        {
            if (plugin == null)
                return false;

            return systemNames
                ?.Any(systemName => plugin.PluginDescriptor.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                ?? false;
        }

        ///// <summary>
        ///// Get plugin logo URL
        ///// </summary>
        ///// <param name="plugin">Plugin</param>
        ///// <returns>
        ///// A task that represents the asynchronous operation
        ///// The task result contains the logo URL
        ///// </returns>
        //public virtual async Task<string> GetPluginLogoUrlAsync(TPlugin plugin)
        //{
        //    return await _pluginService.GetPluginLogoUrlAsync(plugin.PluginDescriptor);
        //}

        #endregion
    }
}