using ARWNI2S.Infrastructure.Entities;

namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Represents a module service
    /// </summary>
    public partial interface IModuleService
    {
        /// <summary>
        /// Get module descriptors
        /// </summary>
        /// <typeparam name="TModule">The type of modules to get</typeparam>
        /// <param name="loadMode">Filter by load modules mode</param>
        /// <param name="user">Filter by  user; pass null to load all records</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all records</param>
        /// <param name="group">Filter by module group; pass null to load all records</param>
        /// <param name="friendlyName">Filter by module friendly name; pass null to load all records</param>
        /// <param name="author">Filter by module author; pass null to load all records</param>
        /// <param name="dependsOnSystemName">System name of the module to define dependencies</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the module descriptors
        /// </returns>
        Task<IList<ModuleDescriptor>> GetModuleDescriptorsAsync<TModule>(LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,
            IUser user = null, int nodeId = 0, string group = null, string dependsOnSystemName = "", string friendlyName = null, string author = null) where TModule : class, IModule;

        /// <summary>
        /// Get a module descriptor by the system name
        /// </summary>
        /// <typeparam name="TModule">The type of module to get</typeparam>
        /// <param name="systemName">Module system name</param>
        /// <param name="loadMode">Load modules mode</param>
        /// <param name="user">Filter by  user; pass null to load all records</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all records</param>
        /// <param name="group">Filter by module group; pass null to load all records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the >Module descriptor
        /// </returns>
        Task<ModuleDescriptor> GetModuleDescriptorBySystemNameAsync<TModule>(string systemName,
            LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,
            IUser user = null, int nodeId = 0, string group = null) where TModule : class, IModule;

        /// <summary>
        /// Get modules
        /// </summary>
        /// <typeparam name="TModule">The type of modules to get</typeparam>
        /// <param name="loadMode">Filter by load modules mode</param>
        /// <param name="user">Filter by  user; pass null to load all records</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all records</param>
        /// <param name="group">Filter by module group; pass null to load all records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the modules
        /// </returns>
        Task<IList<TModule>> GetModulesAsync<TModule>(LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,
            IUser user = null, int nodeId = 0, string group = null) where TModule : class, IModule;

        /// <summary>
        /// Find a module by the type which is located into the same assembly as a module
        /// </summary>
        /// <param name="typeInAssembly">Type</param>
        /// <returns>Module</returns>
        IModule FindModuleByTypeInAssembly(Type typeInAssembly);

        ///// <summary>
        ///// Get module logo URL
        ///// </summary>
        ///// <param name="moduleDescriptor">Module descriptor</param>
        ///// <returns>
        ///// A task that represents the asynchronous operation
        ///// The task result contains the logo URL
        ///// </returns>
        //Task<string> GetModuleLogoUrlAsync(ModuleDescriptor moduleDescriptor);

        /// <summary>
        /// Prepare module to the installation
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <param name="user">User</param>
        /// <param name="checkDependencies">Specifies whether to check module dependencies</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task PrepareModuleToInstallAsync(string systemName, IUser user = null, bool checkDependencies = true);

        /// <summary>
        /// Prepare module to the uninstallation
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task PrepareModuleToUninstallAsync(string systemName);

        /// <summary>
        /// Prepare module to the removing
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task PrepareModuleToDeleteAsync(string systemName);

        /// <summary>
        /// Reset changes
        /// </summary>
        void ResetChanges();

        /// <summary>
        /// Clear installed modules list
        /// </summary>
        void ClearInstalledModulesList();

        /// <summary>
        /// Install modules
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InstallModulesAsync();

        /// <summary>
        /// Uninstall modules
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UninstallModulesAsync();

        /// <summary>
        /// Delete modules
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteModulesAsync();

        /// <summary>
        /// Update modules
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateModulesAsync();

        /// <summary>
        /// Check whether application restart is required to apply changes to modules
        /// </summary>
        /// <returns>Result of check</returns>
        bool IsRestartRequired();

        /// <summary>
        /// Get names of incompatible modules
        /// </summary>
        /// <returns>List of module names</returns>
        IDictionary<string, ModuleIncompatibleType> GetIncompatibleModules();

        /// <summary>
        /// Get all assembly loaded collisions
        /// </summary>
        /// <returns>List of module loaded assembly info</returns>
        IList<ModuleLoadedAssemblyInfo> GetAssemblyCollisions();
    }
}