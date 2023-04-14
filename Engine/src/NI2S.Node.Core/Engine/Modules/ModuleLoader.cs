// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NI2S.Node.Engine
{
    internal class ModuleLoader
    {
        /// <summary>
        /// Get plugin descriptors
        /// </summary>
        /// <typeparam name="TModule">The type of plugins to get</typeparam>
        /// <param name="customer">Filter by  customer; pass null to load all records</param>
        /// <param name="storeId">Filter by store; pass 0 to load all records</param>
        /// <param name="group">Filter by plugin group; pass null to load all records</param>
        /// <param name="friendlyName">Filter by plugin friendly name; pass null to load all records</param>
        /// <param name="author">Filter by plugin author; pass null to load all records</param>
        /// <param name="dependsOnSystemName">System name of the plugin to define dependencies</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the plugin descriptors
        /// </returns>
        public Task<IList<ModuleInfo>> GetModuleDescriptorsAsync<TModule>(/*LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,*/
            string group = null, string dependsOnSystemName = "", string friendlyName = null, string author = null) where TModule : class, IEngineModule
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a plugin descriptor by the system name
        /// </summary>
        /// <typeparam name="TModule">The type of plugin to get</typeparam>
        /// <param name="systemName">Module system name</param>
        /// <param name="customer">Filter by  customer; pass null to load all records</param>
        /// <param name="storeId">Filter by store; pass 0 to load all records</param>
        /// <param name="group">Filter by plugin group; pass null to load all records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the >Module descriptor
        /// </returns>
        public Task<ModuleInfo> GetModuleDescriptorBySystemNameAsync<TModule>(string systemName,
            /*LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,*/
            string group = null) where TModule : class, IEngineModule
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get plugins
        /// </summary>
        /// <typeparam name="TModule">The type of plugins to get</typeparam>
        /// <param name="customer">Filter by  customer; pass null to load all records</param>
        /// <param name="storeId">Filter by store; pass 0 to load all records</param>
        /// <param name="group">Filter by plugin group; pass null to load all records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the plugins
        /// </returns>
        public Task<IList<TModule>> GetModulesAsync<TModule>(/*LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,*/
            string group = null) where TModule : class, IEngineModule
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find a plugin by the type which is located into the same assembly as a plugin
        /// </summary>
        /// <param name="typeInAssembly">Type</param>
        /// <returns>Module</returns>
        public IEngineModule FindModuleByTypeInAssembly(Type typeInAssembly)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get plugin logo URL
        /// </summary>
        /// <param name="pluginDescriptor">Module descriptor</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the logo URL
        /// </returns>
        public Task<string> GetModuleLogoUrlAsync(ModuleInfo pluginDescriptor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare plugin to the installation
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <param name="customer">Customer</param>
        /// <param name="checkDependencies">Specifies whether to check plugin dependencies</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task PrepareModuleToInstallAsync(string systemName, bool checkDependencies = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare plugin to the uninstallation
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task PrepareModuleToUninstallAsync(string systemName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare plugin to the removing
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task PrepareModuleToDeleteAsync(string systemName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reset changes
        /// </summary>
        public void ResetChanges()
        {

        }

        /// <summary>
        /// Clear installed plugins list
        /// </summary>
        public void ClearInstalledModulesList()
        {

        }

        /// <summary>
        /// Install plugins
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task InstallModulesAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uninstall plugins
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task UninstallModulesAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete plugins
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task DeleteModulesAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update plugins
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task UpdateModulesAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check whether application restart is required to apply changes to plugins
        /// </summary>
        /// <returns>Result of check</returns>
        public bool IsRestartRequired()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get names of incompatible plugins
        /// </summary>
        /// <returns>List of plugin names</returns>
        public IDictionary<string, IncompatibilityType> GetIncompatibleModules()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all assembly loaded collisions
        /// </summary>
        /// <returns>List of plugin loaded assembly info</returns>
        public IList<ModuleLoadedAssemblyInfo> GetAssemblyCollisions()
        {
            throw new NotImplementedException();
        }
    }
}
