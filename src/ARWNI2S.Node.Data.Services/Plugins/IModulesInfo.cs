namespace ARWNI2S.Node.Data.Services.Plugins
{
    /// <summary>
    /// Represents an information about modules
    /// </summary>
    public interface IModulesInfo
    {
        /// <summary>
        /// Save modules info to the file
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SaveAsync();

        /// <summary>
        /// Get modules info
        /// </summary>
        /// <returns>
        /// The true if data are loaded, otherwise False
        /// </returns>
        void LoadModuleInfo();

        /// <summary>
        /// Save modules info to the file
        /// </summary>
        void Save();

        /// <summary>
        /// Create copy from another instance of IModulesInfo interface
        /// </summary>
        /// <param name="modulesInfo">Modules info</param>
        void CopyFrom(IModulesInfo modulesInfo);

        /// <summary>
        /// Gets or sets the list of all installed module
        /// </summary>
        IList<ModuleDescriptorBaseInfo> InstalledModules { get; set; }

        /// <summary>
        /// Gets or sets the list of module names which will be uninstalled
        /// </summary>
        IList<string> ModuleNamesToUninstall { get; set; }

        /// <summary>
        /// Gets or sets the list of module names which will be deleted
        /// </summary>
        IList<string> ModuleNamesToDelete { get; set; }

        /// <summary>
        /// Gets or sets the list of module names which will be installed
        /// </summary>
        IList<(string SystemName, Guid? UserGuid)> ModuleNamesToInstall { get; set; }

        /// <summary>
        /// Gets or sets the list of assembly loaded collisions
        /// </summary>
        IList<ModuleLoadedAssemblyInfo> AssemblyLoadedCollision { get; set; }

        /// <summary>
        /// Gets or sets a collection of module descriptors of all deployed modules
        /// </summary>
        IList<(ModuleDescriptor moduleDescriptor, bool needToDeploy)> ModuleDescriptors { get; set; }

        /// <summary>
        /// Gets or sets the list of module which are not compatible with the current version
        /// </summary>
        /// <remarks>
        /// Key - the system name of module.
        /// Value - the incompatibility type.
        /// </remarks>
        IDictionary<string, ModuleIncompatibleType> IncompatibleModules { get; set; }
    }
}