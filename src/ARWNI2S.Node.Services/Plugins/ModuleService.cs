using ARWNI2S.Infrastructure;
using ARWNI2S.Infrastructure.Entities;
using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Data.Extensions;
using ARWNI2S.Node.Data.Migrations;
using ARWNI2S.Node.Services.Localization;
using ARWNI2S.Node.Services.Logging;
using ARWNI2S.Node.Services.Users;
using System.Reflection;
using ARWNI2S.Node.Core.Entities.Clustering;
using ARWNI2S.Node.Core.Network;

namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Represents the module service implementation
    /// </summary>
    public partial class ModuleService : IModuleService
    {
        #region Fields

        private readonly ClusteringSettings _nodeSettings;
        private readonly IUserService _userService;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMigrationManager _migrationManager;
        private readonly ILogService _logger;
        private readonly IEngineFileProvider _fileProvider;
        private readonly IModulesInfo _modulesInfo;
        private readonly INI2SNetHelper _webHelper;
        //private readonly MediaSettings _mediaSettings;

        #endregion

        #region Ctor

        public ModuleService(ClusteringSettings nodeSettings,
            IUserService userService,
            //IHttpContextAccessor httpContextAccessor,
            IMigrationManager migrationManager,
            ILogService logger,
            IEngineFileProvider fileProvider,
            INI2SNetHelper nodeHelper//,
            //MediaSettings mediaSettings
            )
        {
            _nodeSettings = nodeSettings;
            _userService = userService;
            //_httpContextAccessor = httpContextAccessor;
            _migrationManager = migrationManager;
            _logger = logger;
            _fileProvider = fileProvider;
            _modulesInfo = Singleton<IModulesInfo>.Instance;
            _webHelper = nodeHelper;
            //_mediaSettings = mediaSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether to load the module based on the load mode passed
        /// </summary>
        /// <param name="moduleDescriptor">Module descriptor to check</param>
        /// <param name="loadMode">Load modules mode</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByLoadMode(ModuleDescriptor moduleDescriptor, LoadModulesMode loadMode)
        {
            ArgumentNullException.ThrowIfNull(moduleDescriptor);

            return loadMode switch
            {
                LoadModulesMode.All => true,
                LoadModulesMode.InstalledOnly => moduleDescriptor.Installed,
                LoadModulesMode.NotInstalledOnly => !moduleDescriptor.Installed,
                _ => throw new NotSupportedException(nameof(loadMode)),
            };
        }

        /// <summary>
        /// Check whether to load the module based on the module group passed
        /// </summary>
        /// <param name="moduleDescriptor">Module descriptor to check</param>
        /// <param name="group">Group name</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByModuleGroup(ModuleDescriptor moduleDescriptor, string group)
        {
            ArgumentNullException.ThrowIfNull(moduleDescriptor);

            if (string.IsNullOrEmpty(group))
                return true;

            return group.Equals(moduleDescriptor.Group, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Check whether to load the module based on the user passed
        /// </summary>
        /// <param name="moduleDescriptor">Module descriptor to check</param>
        /// <param name="user">User</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result of check
        /// </returns>
        protected virtual async Task<bool> FilterByUserAsync(ModuleDescriptor moduleDescriptor, INI2SUser user)
        {
            ArgumentNullException.ThrowIfNull(moduleDescriptor);

            if (user == null || !moduleDescriptor.LimitedToUserRoles.Any())
                return true;

            if (_nodeSettings.IgnoreAcl)
                return true;

            return moduleDescriptor.LimitedToUserRoles.Intersect(await _userService.GetUserRoleIdsAsync((User)user)).Any();
        }

        /// <summary>
        /// Check whether to load the module based on the node identifier passed
        /// </summary>
        /// <param name="moduleDescriptor">Module descriptor to check</param>
        /// <param name="nodeId">Node identifier</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByNode(ModuleDescriptor moduleDescriptor, int nodeId)
        {
            ArgumentNullException.ThrowIfNull(moduleDescriptor);

            //no validation required
            if (nodeId == 0)
                return true;

            if (!moduleDescriptor.LimitedToNodes.Any())
                return true;

            return moduleDescriptor.LimitedToNodes.Contains(nodeId);
        }

        /// <summary>
        /// Check whether to load the module based on dependency from other module
        /// </summary>
        /// <param name="moduleDescriptor">Module descriptor to check</param>
        /// <param name="dependsOnSystemName">Other module system name</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByDependsOn(ModuleDescriptor moduleDescriptor, string dependsOnSystemName)
        {
            ArgumentNullException.ThrowIfNull(moduleDescriptor);

            if (string.IsNullOrEmpty(dependsOnSystemName))
                return true;

            return moduleDescriptor.DependsOn?.Contains(dependsOnSystemName) ?? false;
        }

        /// <summary>
        /// Check whether to load the module based on the module friendly name passed
        /// </summary>
        /// <param name="moduleDescriptor">Module descriptor to check</param>
        /// <param name="friendlyName">Module friendly name</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByModuleFriendlyName(ModuleDescriptor moduleDescriptor, string friendlyName)
        {
            ArgumentNullException.ThrowIfNull(moduleDescriptor);

            if (string.IsNullOrEmpty(friendlyName))
                return true;

            return moduleDescriptor.FriendlyName.Contains(friendlyName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Check whether to load the module based on the module author passed
        /// </summary>
        /// <param name="moduleDescriptor">Module descriptor to check</param>
        /// <param name="author">Module author</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByModuleAuthor(ModuleDescriptor moduleDescriptor, string author)
        {
            ArgumentNullException.ThrowIfNull(moduleDescriptor);

            if (string.IsNullOrEmpty(author))
                return true;

            return moduleDescriptor.Author.Contains(author, StringComparison.InvariantCultureIgnoreCase);
        }

        protected virtual void DeleteModuleData(Type moduleType)
        {
            var assembly = Assembly.GetAssembly(moduleType);
            _migrationManager.ApplyDownMigrations(assembly);
        }

        protected virtual void InsertModuleData(Type moduleType, MigrationProcessType migrationProcessType = MigrationProcessType.NoMatter)
        {
            var assembly = Assembly.GetAssembly(moduleType);
            _migrationManager.ApplyUpMigrations(assembly, migrationProcessType);

            //mark update migrations as applied
            if (migrationProcessType == MigrationProcessType.Installation)
            {
                _migrationManager.ApplyUpMigrations(assembly, MigrationProcessType.Update, true);
            }
        }

        protected virtual bool ModulesUploaded()
        {
            var modulesDirectories =
                _fileProvider.GetDirectories(_fileProvider.MapPath(NI2SModuleDefaults.UploadedPath));

            if (modulesDirectories.Length == 0)
                return false;

            return modulesDirectories.Any(d =>
                _fileProvider.GetFiles(d, "*.dll").Length != 0 || _fileProvider.GetFiles(d, "module.json").Length != 0);
        }

        #endregion

        #region Methods

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
        public virtual async Task<IList<ModuleDescriptor>> GetModuleDescriptorsAsync<TModule>(LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,
            INI2SUser user = null, int nodeId = 0, string group = null, string dependsOnSystemName = "", string friendlyName = null, string author = null) where TModule : class, IModule
        {
            var moduleDescriptors = _modulesInfo.ModuleDescriptors.Select(p => p.moduleDescriptor).ToList();

            //filter modules
            moduleDescriptors = await moduleDescriptors.WhereAwait(async descriptor =>
                FilterByLoadMode(descriptor, loadMode) &&
                await FilterByUserAsync(descriptor, user) &&
                FilterByNode(descriptor, nodeId) &&
                FilterByModuleGroup(descriptor, group) &&
                FilterByDependsOn(descriptor, dependsOnSystemName) &&
                FilterByModuleFriendlyName(descriptor, friendlyName) &&
                FilterByModuleAuthor(descriptor, author)).ToListAsync();

            //filter by the passed type
            if (typeof(TModule) != typeof(IModule))
                moduleDescriptors = moduleDescriptors.Where(descriptor => typeof(TModule).IsAssignableFrom(descriptor.ModuleType)).ToList();

            //order by group name
            moduleDescriptors = moduleDescriptors.OrderBy(descriptor => descriptor.Group)
                .ThenBy(descriptor => descriptor.DisplayOrder).ToList();

            return moduleDescriptors;
        }

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
        public virtual async Task<ModuleDescriptor> GetModuleDescriptorBySystemNameAsync<TModule>(string systemName,
            LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,
            INI2SUser user = null, int nodeId = 0, string @group = null) where TModule : class, IModule
        {
            return (await GetModuleDescriptorsAsync<TModule>(loadMode, user, nodeId, group))
                .FirstOrDefault(descriptor => descriptor.SystemName.Equals(systemName));
        }

        /// <summary>
        /// Get modules
        /// </summary>
        /// <typeparam name="TModule">The type of modules to get</typeparam>
        /// <param name="loadMode">Filter by load modules mode</param>
        /// <param name="user">Filter by user; pass null to load all records</param>
        /// <param name="nodeId">Filter by node; pass 0 to load all records</param>
        /// <param name="group">Filter by module group; pass null to load all records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the modules
        /// </returns>
        public virtual async Task<IList<TModule>> GetModulesAsync<TModule>(
            LoadModulesMode loadMode = LoadModulesMode.InstalledOnly,
            INI2SUser user = null, int nodeId = 0, string @group = null) where TModule : class, IModule
        {
            return (await GetModuleDescriptorsAsync<TModule>(loadMode, user, nodeId, group))
                .Select(descriptor => descriptor.Instance<TModule>()).ToList();
        }

        /// <summary>
        /// Find a module by the type which is located into the same assembly as a module
        /// </summary>
        /// <param name="typeInAssembly">Type</param>
        /// <returns>Module</returns>
        public virtual IModule FindModuleByTypeInAssembly(Type typeInAssembly)
        {
            ArgumentNullException.ThrowIfNull(typeInAssembly);

            //try to do magic
            var moduleDescriptor = _modulesInfo.ModuleDescriptors.FirstOrDefault(descriptor =>
                descriptor.moduleDescriptor?.ReferencedAssembly?.FullName?.Equals(typeInAssembly.Assembly.FullName,
                    StringComparison.InvariantCultureIgnoreCase) ?? false);

            return moduleDescriptor.moduleDescriptor?.Instance<IModule>();
        }

        ///// <summary>
        ///// Get module logo URL
        ///// </summary>
        ///// <param name="moduleDescriptor">Module descriptor</param>
        ///// <returns>
        ///// A task that represents the asynchronous operation
        ///// The task result contains the logo URL
        ///// </returns>
        //public virtual Task<string> GetModuleLogoUrlAsync(ModuleDescriptor moduleDescriptor)
        //{
        //    var moduleDirectory = _fileProvider.GetDirectoryName(moduleDescriptor.OriginalAssemblyFile);
        //    if (string.IsNullOrEmpty(moduleDirectory))
        //        return Task.FromResult<string>(null);

        //    //check for supported extensions
        //    var logoExtension = ModuleServicesDefaults.SupportedLogoImageExtensions
        //        .FirstOrDefault(ext => _fileProvider.FileExists(_fileProvider.Combine(moduleDirectory, $"{ModuleServicesDefaults.LogoFileName}.{ext}")));
        //    if (string.IsNullOrWhiteSpace(logoExtension))
        //        return Task.FromResult<string>(null);

        //    var pathBase = _httpContextAccessor.HttpContext.Request.PathBase.Value ?? string.Empty;
        //    var logoPathUrl = _mediaSettings.UseAbsoluteImagePath ? _webHelper.GetNodeLocation() : $"{pathBase}/";

        //    var logoUrl = $"{logoPathUrl}{ModuleServicesDefaults.PathName}/" +
        //        $"{_fileProvider.GetDirectoryNameOnly(moduleDirectory)}/{ModuleServicesDefaults.LogoFileName}.{logoExtension}";

        //    return Task.FromResult(logoUrl);
        //}

        /// <summary>
        /// Prepare module to the installation
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <param name="user">User</param>
        /// <param name="checkDependencies">Specifies whether to check module dependencies</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task PrepareModuleToInstallAsync(string systemName, INI2SUser user = null, bool checkDependencies = true)
        {
            //add module name to the appropriate list (if not yet contained) and save changes
            if (_modulesInfo.ModuleNamesToInstall.Any(item => item.SystemName == systemName))
                return;

            var modulesAfterRestart = _modulesInfo.InstalledModules.Select(pd => pd.SystemName).Where(installedSystemName => !_modulesInfo.ModuleNamesToUninstall.Contains(installedSystemName)).ToList();
            modulesAfterRestart.AddRange(_modulesInfo.ModuleNamesToInstall.Select(item => item.SystemName));

            if (checkDependencies)
            {
                var descriptor = await GetModuleDescriptorBySystemNameAsync<IModule>(systemName, LoadModulesMode.NotInstalledOnly);

                if (descriptor.DependsOn?.Any() ?? false)
                {
                    var dependsOn = descriptor.DependsOn
                        .Where(dependsOnSystemName => !modulesAfterRestart.Contains(dependsOnSystemName)).ToList();

                    if (dependsOn.Count != 0)
                    {
                        var dependsOnSystemNames = dependsOn.Aggregate((all, current) => $"{all}, {current}");

                        //do not inject services via constructor because it'll cause circular references
                        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

                        var errorMessage = string.Format(await localizationService.GetResourceAsync("Admin.Modules.Errors.InstallDependsOn"), string.IsNullOrEmpty(descriptor.FriendlyName) ? descriptor.SystemName : descriptor.FriendlyName, dependsOnSystemNames);

                        throw new NodeException(errorMessage);
                    }
                }
            }

            _modulesInfo.ModuleNamesToInstall.Add((systemName, ((User)user)?.UserGuid));
            await _modulesInfo.SaveAsync();
        }

        /// <summary>
        /// Prepare module to the uninstallation
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task PrepareModuleToUninstallAsync(string systemName)
        {
            //add module name to the appropriate list (if not yet contained) and save changes
            if (_modulesInfo.ModuleNamesToUninstall.Contains(systemName))
                return;

            var dependentModules = await GetModuleDescriptorsAsync<IModule>(dependsOnSystemName: systemName);
            var descriptor = await GetModuleDescriptorBySystemNameAsync<IModule>(systemName);

            if (dependentModules.Any())
            {
                var dependsOn = new List<string>();

                foreach (var dependentModule in dependentModules)
                {
                    if (!_modulesInfo.InstalledModules.Select(pd => pd.SystemName).Contains(dependentModule.SystemName))
                        continue;
                    if (_modulesInfo.ModuleNamesToUninstall.Contains(dependentModule.SystemName))
                        continue;

                    dependsOn.Add(string.IsNullOrEmpty(dependentModule.FriendlyName)
                        ? dependentModule.SystemName
                        : dependentModule.FriendlyName);
                }

                if (dependsOn.Count != 0)
                {
                    var dependsOnSystemNames = dependsOn.Aggregate((all, current) => $"{all}, {current}");

                    //do not inject services via constructor because it'll cause circular references
                    var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

                    var errorMessage = string.Format(await localizationService.GetResourceAsync("Admin.Modules.Errors.UninstallDependsOn"),
                        string.IsNullOrEmpty(descriptor.FriendlyName) ? descriptor.SystemName : descriptor.FriendlyName,
                        dependsOnSystemNames);

                    throw new NodeException(errorMessage);
                }
            }

            var module = descriptor?.Instance<IModule>();

            if (module != null)
                await module.PrepareModuleToUninstallAsync();

            _modulesInfo.ModuleNamesToUninstall.Add(systemName);
            await _modulesInfo.SaveAsync();
        }

        /// <summary>
        /// Prepare module to the removing
        /// </summary>
        /// <param name="systemName">Module system name</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task PrepareModuleToDeleteAsync(string systemName)
        {
            //add module name to the appropriate list (if not yet contained) and save changes
            if (_modulesInfo.ModuleNamesToDelete.Contains(systemName))
                return;

            _modulesInfo.ModuleNamesToDelete.Add(systemName);
            await _modulesInfo.SaveAsync();
        }

        /// <summary>
        /// Reset changes
        /// </summary>
        public virtual void ResetChanges()
        {
            //clear lists and save changes
            _modulesInfo.ModuleNamesToDelete.Clear();
            _modulesInfo.ModuleNamesToInstall.Clear();
            _modulesInfo.ModuleNamesToUninstall.Clear();
            _modulesInfo.Save();

            //display all modules on the module list page
            var moduleDescriptors = _modulesInfo.ModuleDescriptors.ToList();
            foreach (var moduleDescriptor in moduleDescriptors)
                moduleDescriptor.moduleDescriptor.ShowInModulesList = true;

            //clear the uploaded directory
            foreach (var directory in _fileProvider.GetDirectories(_fileProvider.MapPath(NI2SModuleDefaults.UploadedPath)))
                _fileProvider.DeleteDirectory(directory);
        }

        /// <summary>
        /// Clear installed modules list
        /// </summary>
        public virtual void ClearInstalledModulesList()
        {
            _modulesInfo.InstalledModules.Clear();
        }

        /// <summary>
        /// Install modules
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InstallModulesAsync()
        {
            //get all uninstalled modules
            var moduleDescriptors = _modulesInfo.ModuleDescriptors.Where(descriptor => !descriptor.moduleDescriptor.Installed).ToList();

            //filter modules need to install
            moduleDescriptors = moduleDescriptors.Where(descriptor => _modulesInfo.ModuleNamesToInstall
                .Any(item => item.SystemName.Equals(descriptor.moduleDescriptor.SystemName))).ToList();
            if (moduleDescriptors.Count == 0)
                return;

            //do not inject services via constructor because it'll cause circular references
            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            var userActivityService = EngineContext.Current.Resolve<IUserActivityService>();

            //install modules
            foreach (var descriptor in moduleDescriptors.OrderBy(moduleDescriptor => moduleDescriptor.moduleDescriptor.DisplayOrder))
            {
                try
                {
                    InsertModuleData(descriptor.moduleDescriptor.ModuleType, MigrationProcessType.Installation);

                    //try to install an instance
                    await descriptor.moduleDescriptor.Instance<IModule>().InstallAsync();

                    //remove and add module system name to appropriate lists
                    var moduleToInstall = _modulesInfo.ModuleNamesToInstall
                        .FirstOrDefault(module => module.SystemName.Equals(descriptor.moduleDescriptor.SystemName));
                    _modulesInfo.InstalledModules.Add(descriptor.moduleDescriptor.GetBaseInfoCopy);
                    _modulesInfo.ModuleNamesToInstall.Remove(moduleToInstall);

                    //activity log
                    var user = await _userService.GetUserByGuidAsync(moduleToInstall.UserGuid ?? Guid.Empty);
                    await userActivityService.InsertActivityAsync(user, "InstallNewModule",
                        string.Format(await localizationService.GetResourceAsync("ActivityLog.InstallNewModule"), descriptor.moduleDescriptor.SystemName));

                    //mark the module as installed
                    descriptor.moduleDescriptor.Installed = true;
                    descriptor.moduleDescriptor.ShowInModulesList = true;
                }
                catch (Exception exception)
                {
                    //log error
                    var message = string.Format(await localizationService.GetResourceAsync("Admin.Modules.Errors.NotInstalled"), descriptor.moduleDescriptor.SystemName);
                    await _logger.ErrorAsync(message, exception);
                }
            }

            //save changes
            await _modulesInfo.SaveAsync();
        }

        /// <summary>
        /// Uninstall modules
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UninstallModulesAsync()
        {
            //get all installed modules
            var moduleDescriptors = _modulesInfo.ModuleDescriptors.Where(descriptor => descriptor.moduleDescriptor.Installed).ToList();

            //filter modules need to uninstall
            moduleDescriptors = moduleDescriptors
                .Where(descriptor => _modulesInfo.ModuleNamesToUninstall.Contains(descriptor.moduleDescriptor.SystemName)).ToList();
            if (moduleDescriptors.Count == 0)
                return;

            //do not inject services via constructor because it'll cause circular references
            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            var userActivityService = EngineContext.Current.Resolve<IUserActivityService>();

            //uninstall modules
            foreach (var descriptor in moduleDescriptors.OrderByDescending(moduleDescriptor => moduleDescriptor.moduleDescriptor.DisplayOrder))
            {
                try
                {
                    var module = descriptor.moduleDescriptor.Instance<IModule>();
                    //try to uninstall an instance
                    await module.UninstallAsync();

                    //clear module data on the database
                    DeleteModuleData(descriptor.moduleDescriptor.ModuleType);

                    //remove module system name from appropriate lists
                    _modulesInfo.InstalledModules.Remove(descriptor.moduleDescriptor);
                    _modulesInfo.ModuleNamesToUninstall.Remove(descriptor.moduleDescriptor.SystemName);

                    //activity log
                    await userActivityService.InsertActivityAsync(SystemKeywords.AdminArea.UninstallModule,
                        string.Format(await localizationService.GetResourceAsync("ActivityLog.UninstallModule"), descriptor.moduleDescriptor.SystemName));

                    //mark the module as uninstalled
                    descriptor.moduleDescriptor.Installed = false;
                    descriptor.moduleDescriptor.ShowInModulesList = true;
                }
                catch (Exception exception)
                {
                    //log error
                    var message = string.Format(await localizationService.GetResourceAsync("Admin.Modules.Errors.NotUninstalled"), descriptor.moduleDescriptor.SystemName);
                    await _logger.ErrorAsync(message, exception);
                }
            }

            //save changes
            await _modulesInfo.SaveAsync();
        }

        /// <summary>
        /// Delete modules
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteModulesAsync()
        {
            //get all uninstalled modules (delete module only previously uninstalled)
            var moduleDescriptors = _modulesInfo.ModuleDescriptors.Where(descriptor => !descriptor.moduleDescriptor.Installed).ToList();

            //filter modules need to delete
            moduleDescriptors = moduleDescriptors
                .Where(descriptor => _modulesInfo.ModuleNamesToDelete.Contains(descriptor.moduleDescriptor.SystemName)).ToList();
            if (moduleDescriptors.Count == 0)
                return;

            //do not inject services via constructor because it'll cause circular references
            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            var userActivityService = EngineContext.Current.Resolve<IUserActivityService>();

            //delete modules
            foreach (var (moduleDescriptor, needToDeploy) in moduleDescriptors)
            {
                try
                {
                    //try to delete a module directory from disk storage
                    var moduleDirectory = _fileProvider.GetDirectoryName(moduleDescriptor.OriginalAssemblyFile);
                    if (_fileProvider.DirectoryExists(moduleDirectory))
                        _fileProvider.DeleteDirectory(moduleDirectory);

                    //remove module system name from the appropriate list
                    _modulesInfo.ModuleNamesToDelete.Remove(moduleDescriptor.SystemName);

                    //activity log
                    await userActivityService.InsertActivityAsync(SystemKeywords.AdminArea.DeleteModule,
                        string.Format(await localizationService.GetResourceAsync("ActivityLog.DeleteModule"), moduleDescriptor.SystemName));
                }
                catch (Exception exception)
                {
                    //log error
                    var message = string.Format(await localizationService.GetResourceAsync("Admin.Modules.Errors.NotDeleted"), moduleDescriptor.SystemName);
                    await _logger.ErrorAsync(message, exception);
                }
            }

            //save changes
            await _modulesInfo.SaveAsync();
        }

        /// <summary>
        /// Check whether application restart is required to apply changes to modules
        /// </summary>
        /// <returns>Result of check</returns>
        public virtual bool IsRestartRequired()
        {
            //return true if any of lists contains items or some modules were uploaded
            return _modulesInfo.ModuleNamesToInstall.Any()
                   || _modulesInfo.ModuleNamesToUninstall.Any()
                   || _modulesInfo.ModuleNamesToDelete.Any()
                   || ModulesUploaded();
        }

        /// <summary>
        /// Update modules
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateModulesAsync()
        {
            foreach (var installedModule in _modulesInfo.InstalledModules)
            {
                var (moduleDescriptor, needToDeploy) = _modulesInfo.ModuleDescriptors.FirstOrDefault(pd =>
                    pd.moduleDescriptor.SystemName.Equals(installedModule.SystemName, StringComparison.InvariantCultureIgnoreCase));

                if (moduleDescriptor == null)
                    continue;

                if (installedModule.Version == moduleDescriptor.Version)
                    continue;

                //run new migrations from the module if there are exists
                InsertModuleData(moduleDescriptor.ModuleType, MigrationProcessType.Update);

                //run the module update logic
                await moduleDescriptor.Instance<IModule>().UpdateAsync(installedModule.Version, moduleDescriptor.Version);

                //update installed module info
                installedModule.Version = moduleDescriptor.Version;
            }

            await _modulesInfo.SaveAsync();
        }

        /// <summary>
        /// Get names of incompatible modules
        /// </summary>
        /// <returns>List of module names</returns>
        public virtual IDictionary<string, ModuleIncompatibleType> GetIncompatibleModules()
        {
            return _modulesInfo.IncompatibleModules;
        }

        /// <summary>
        /// Get all assembly loaded collisions
        /// </summary>
        /// <returns>List of module loaded assembly info</returns>
        public virtual IList<ModuleLoadedAssemblyInfo> GetAssemblyCollisions()
        {
            return _modulesInfo.AssemblyLoadedCollision;
        }

        #endregion
    }
}