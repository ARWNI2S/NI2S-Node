using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Data.Services.Users;

namespace ARWNI2S.Node.Data.Services.Plugins
{
    /// <summary>
    /// Represents a module manager implementation
    /// </summary>
    /// <typeparam name="TModule">Type of module</typeparam>
    public partial class ModuleManager<TModule> : IModuleManager<TModule> where TModule : class, IModule
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IModuleService _moduleService;

        private readonly Dictionary<string, IList<TModule>> _modules = [];

        #endregion

        #region Ctor

        public ModuleManager(IUserService userService,
            IModuleService moduleService)
        {
            _userService = userService;
            _moduleService = moduleService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare the dictionary key to server loaded modules
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="serverId">Server identifier</param>
        /// <param name="systemName">Module system name</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the key
        /// </returns>
        protected virtual async Task<string> GetKeyAsync(User user, int serverId, string systemName = null)
        {
            return $"{serverId}-{(user != null ? string.Join(',', await _userService.GetUserRoleIdsAsync(user)) : null)}-{systemName}";
        }

        /// <summary>
        /// Load primary active module
        /// </summary>
        /// <param name="systemName">System name of primary active module</param>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="serverId">Filter by server; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the module
        /// </returns>
        protected virtual async Task<TModule> LoadPrimaryModuleAsync(string systemName, User user = null, int serverId = 0)
        {
            //try to get a module by system name or return the first loaded one (it's necessary to have a primary active module)
            var module = await LoadModuleBySystemNameAsync(systemName, user, serverId)
                         ?? (await LoadAllModulesAsync(user, serverId)).FirstOrDefault();

            return module;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load all modules
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="serverId">Filter by server; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of modules
        /// </returns>
        public virtual async Task<IList<TModule>> LoadAllModulesAsync(User user = null, int serverId = 0)
        {
            //get modules and put them into the dictionary to avoid further loading
            var key = await GetKeyAsync(user, serverId);

            if (!_modules.TryGetValue(key, out var _))
                _modules.Add(key, await _moduleService.GetModulesAsync<TModule>(user: user, serverId: serverId));

            return _modules[key];
        }

        /// <summary>
        /// Load module by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="serverId">Filter by server; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the module
        /// </returns>
        public virtual async Task<TModule> LoadModuleBySystemNameAsync(string systemName, User user = null, int serverId = 0)
        {
            if (string.IsNullOrEmpty(systemName))
                return null;

            //try to get already loaded module
            var key = await GetKeyAsync(user, serverId, systemName);
            if (_modules.TryGetValue(key, out var modules1))
                return modules1.FirstOrDefault();

            //or get it from list of all loaded modules or load it for the first time
            var moduleBySystemName = _modules.TryGetValue(await GetKeyAsync(user, serverId), out var modules2)
                && modules2.FirstOrDefault(module =>
                    module.ModuleDescriptor.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) is TModule loadedModule
                ? loadedModule
                : (await _moduleService.GetModuleDescriptorBySystemNameAsync<TModule>(systemName, user: user, serverId: serverId))?.Instance<TModule>();

            _modules.Add(key, [moduleBySystemName]);

            return moduleBySystemName;
        }

        /// <summary>
        /// Load active modules
        /// </summary>
        /// <param name="systemNames">System names of active modules</param>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="serverId">Filter by server; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of active modules
        /// </returns>
        public virtual async Task<IList<TModule>> LoadActiveModulesAsync(List<string> systemNames, User user = null, int serverId = 0)
        {
            if (systemNames == null)
                return [];

            //get loaded modules according to passed system names
            return (await LoadAllModulesAsync(user, serverId))
                .Where(module => systemNames.Contains(module.ModuleDescriptor.SystemName, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Check whether the passed module is active
        /// </summary>
        /// <param name="module">Module to check</param>
        /// <param name="systemNames">System names of active modules</param>
        /// <returns>Result</returns>
        public virtual bool IsModuleActive(TModule module, List<string> systemNames)
        {
            if (module == null)
                return false;

            return systemNames
                ?.Any(systemName => module.ModuleDescriptor.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                ?? false;
        }

        ///// <summary>
        ///// Get module logo URL
        ///// </summary>
        ///// <param name="module">Module</param>
        ///// <returns>
        ///// A task that represents the asynchronous operation
        ///// The task result contains the logo URL
        ///// </returns>
        //public virtual async Task<string> GetModuleLogoUrlAsync(TModule module)
        //{
        //    return await _moduleService.GetModuleLogoUrlAsync(module.ModuleDescriptor);
        //}

        #endregion
    }
}