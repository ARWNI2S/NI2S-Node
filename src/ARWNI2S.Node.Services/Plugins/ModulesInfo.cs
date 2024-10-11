using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using ARWNI2S.Node.Core;
using ARWNI2S.Infrastructure;

namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Represents an information about modules
    /// </summary>
    public partial class ModulesInfo : IModulesInfo
    {
        #region Fields

        private IList<ModuleDescriptorBaseInfo> _installedModules = [];

        protected readonly IEngineFileProvider _fileProvider;

        #endregion

        #region Utilities

        /// <summary>
        /// Deserialize ModuleInfo from json
        /// </summary>
        /// <param name="json">Json data of ModuleInfo</param>
        /// <returns>True if data are loaded, otherwise False</returns>
        protected virtual void DeserializeModuleInfo(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            var modulesInfo = JsonConvert.DeserializeObject<ModulesInfo>(json);

            if (modulesInfo == null)
                return;

            InstalledModules = modulesInfo.InstalledModules;
            ModuleNamesToUninstall = modulesInfo.ModuleNamesToUninstall;
            ModuleNamesToDelete = modulesInfo.ModuleNamesToDelete;
            ModuleNamesToInstall = modulesInfo.ModuleNamesToInstall;
        }

        /// <summary>
        /// Check whether the directory is a module directory
        /// </summary>
        /// <param name="directoryName">Directory name</param>
        /// <returns>Result of check</returns>
        protected bool IsModuleDirectory(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName))
                return false;

            //get parent directory
            var parent = _fileProvider.GetParentDirectory(directoryName);
            if (string.IsNullOrEmpty(parent))
                return false;

            //directory is directly in modules directory
            if (!_fileProvider.GetDirectoryNameOnly(parent).Equals(ModuleServicesDefaults.PathName, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// Get list of description files-module descriptors pairs
        /// </summary>
        /// <param name="directoryName">Module directory name</param>
        /// <returns>Original and parsed description files</returns>
        protected IList<(string DescriptionFile, ModuleDescriptor ModuleDescriptor)> GetDescriptionFilesAndDescriptors(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName))
                throw new ArgumentNullException(nameof(directoryName));

            var result = new List<(string DescriptionFile, ModuleDescriptor ModuleDescriptor)>();

            //try to find description files in the module directory
            var files = _fileProvider.GetFiles(directoryName, ModuleServicesDefaults.DescriptionFileName, false);

            //populate result list
            foreach (var descriptionFile in files)
            {
                //skip files that are not in the module directory
                if (!IsModuleDirectory(_fileProvider.GetDirectoryName(descriptionFile)))
                    continue;

                //load module descriptor from the file
                var text = _fileProvider.ReadAllText(descriptionFile, Encoding.UTF8);
                var moduleDescriptor = ModuleDescriptor.GetModuleDescriptorFromText(text);

                result.Add((descriptionFile, moduleDescriptor));
            }

            //sort list by display order. NOTE: Lowest DisplayOrder will be first i.e 0 , 1, 1, 1, 5, 10
            //it's required: https://www.dragoncorp.org/boards/topic/17455/load-modules-based-on-their-displayorder-on-startup
            result = result.OrderBy(item => item.ModuleDescriptor.DisplayOrder).ToList();

            return result;
        }

        #endregion

        #region Ctor

        public ModulesInfo(IEngineFileProvider fileProvider)
        {
            _fileProvider = fileProvider ?? CommonHelper.DefaultFileProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get modules info
        /// </summary>
        /// <returns>
        /// The true if data are loaded, otherwise False
        /// </returns>
        public virtual void LoadModuleInfo()
        {
            //check whether modules info file exists
            var filePath = _fileProvider.MapPath(ModuleServicesDefaults.ModulesInfoFilePath);

            //try to get module info from the JSON file
            var text = _fileProvider.FileExists(filePath)
                ? _fileProvider.ReadAllText(filePath, Encoding.UTF8)
                : string.Empty;

            DeserializeModuleInfo(text);

            var moduleDescriptors = new List<(ModuleDescriptor moduleDescriptor, bool needToDeploy)>();
            var incompatibleModules = new Dictionary<string, ModuleIncompatibleType>();

            //ensure modules directory is created
            var modulesDirectory = _fileProvider.MapPath(ModuleServicesDefaults.Path);
            _fileProvider.CreateDirectory(modulesDirectory);

            //load module descriptors from the module directory
            foreach (var item in GetDescriptionFilesAndDescriptors(modulesDirectory))
            {
                var descriptionFile = item.DescriptionFile;
                var moduleDescriptor = item.ModuleDescriptor;

                //skip descriptor of module that is going to be deleted
                if (ModuleNamesToDelete.Contains(moduleDescriptor.SystemName))
                    continue;

                //ensure that module is compatible with the current version
                if (!moduleDescriptor.SupportedVersions.Contains(NI2SVersion.CURRENT_VERSION, StringComparer.InvariantCultureIgnoreCase))
                {
                    incompatibleModules.Add(moduleDescriptor.SystemName, ModuleIncompatibleType.NotCompatibleWithCurrentVersion);
                    continue;
                }

                //some more validation
                if (string.IsNullOrEmpty(moduleDescriptor.SystemName?.Trim()))
                    throw new NodeException($"A module '{descriptionFile}' has no system name. Try assigning the module a unique name and recompiling.");

                if (moduleDescriptors.Any(p => p.moduleDescriptor.Equals(moduleDescriptor)))
                    throw new NodeException($"A module with '{moduleDescriptor.SystemName}' system name is already defined");

                //set 'Installed' property
                moduleDescriptor.Installed = InstalledModules.Select(pd => pd.SystemName)
                    .Any(moduleName => moduleName.Equals(moduleDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase));

                try
                {
                    //try to get module directory
                    var moduleDirectory = _fileProvider.GetDirectoryName(descriptionFile);
                    if (string.IsNullOrEmpty(moduleDirectory))
                        throw new NodeException($"Directory cannot be resolved for '{_fileProvider.GetFileName(descriptionFile)}' description file");

                    //get list of all library files in the module directory (not in the bin one)
                    moduleDescriptor.ModuleFiles = _fileProvider.GetFiles(moduleDirectory, "*.dll", false)
                        .Where(file => IsModuleDirectory(_fileProvider.GetDirectoryName(file)))
                        .ToList();

                    //try to find a main module assembly file
                    var mainModuleFile = moduleDescriptor.ModuleFiles.FirstOrDefault(file =>
                    {
                        var fileName = _fileProvider.GetFileName(file);
                        return fileName.Equals(moduleDescriptor.AssemblyFileName, StringComparison.InvariantCultureIgnoreCase);
                    });

                    //file with the specified name not found
                    if (mainModuleFile == null)
                    {
                        //so module is incompatible
                        incompatibleModules.Add(moduleDescriptor.SystemName, ModuleIncompatibleType.MainAssemblyNotFound);
                        continue;
                    }

                    var moduleName = moduleDescriptor.SystemName;

                    //if it's found, set it as original assembly file
                    moduleDescriptor.OriginalAssemblyFile = mainModuleFile;

                    //need to deploy if module is already installed
                    var needToDeploy = InstalledModules.Select(pd => pd.SystemName).Contains(moduleName);

                    //also, deploy if the module is only going to be installed now
                    needToDeploy = needToDeploy || ModuleNamesToInstall.Any(moduleInfo => moduleInfo.SystemName.Equals(moduleName));

                    //finally, exclude from deploying the module that is going to be deleted
                    needToDeploy = needToDeploy && !ModuleNamesToDelete.Contains(moduleName);

                    //mark module as successfully deployed
                    moduleDescriptors.Add((moduleDescriptor, needToDeploy));
                }
                catch (ReflectionTypeLoadException exception)
                {
                    //get all loader exceptions
                    var error = exception.LoaderExceptions.Aggregate($"Module '{moduleDescriptor.FriendlyName}'. ",
                        (message, nextMessage) => $"{message}{nextMessage?.Message ?? string.Empty}{Environment.NewLine}");

                    throw new NodeException(error, exception);
                }
                catch (Exception exception)
                {
                    //add a module name, this way we can easily identify a problematic module
                    throw new NodeException($"Module '{moduleDescriptor.FriendlyName}'. {exception.Message}", exception);
                }
            }

            IncompatibleModules = incompatibleModules;
            ModuleDescriptors = moduleDescriptors;
        }


        /// <summary>
        /// Save modules info to the file
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SaveAsync()
        {
            //save the file
            var filePath = _fileProvider.MapPath(ModuleServicesDefaults.ModulesInfoFilePath);
            var text = JsonConvert.SerializeObject(this, Formatting.Indented);
            await _fileProvider.WriteAllTextAsync(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// Save modules info to the file
        /// </summary>
        public virtual void Save()
        {
            //save the file
            var filePath = _fileProvider.MapPath(ModuleServicesDefaults.ModulesInfoFilePath);
            var text = JsonConvert.SerializeObject(this, Formatting.Indented);
            _fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// Create copy from another instance of IModulesInfo interface
        /// </summary>
        /// <param name="modulesInfo">Modules info</param>
        public virtual void CopyFrom(IModulesInfo modulesInfo)
        {
            InstalledModules = modulesInfo.InstalledModules?.ToList() ?? [];
            ModuleNamesToUninstall = modulesInfo.ModuleNamesToUninstall?.ToList() ?? [];
            ModuleNamesToDelete = modulesInfo.ModuleNamesToDelete?.ToList() ?? [];
            ModuleNamesToInstall = modulesInfo.ModuleNamesToInstall?.ToList() ?? [];
            AssemblyLoadedCollision = modulesInfo.AssemblyLoadedCollision?.ToList();
            ModuleDescriptors = modulesInfo.ModuleDescriptors;
            IncompatibleModules = modulesInfo.IncompatibleModules?.ToDictionary(item => item.Key, item => item.Value);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of all installed module
        /// </summary>
        public virtual IList<ModuleDescriptorBaseInfo> InstalledModules
        {
            get
            {
                if (_installedModules?.Any() ?? false)
                    return _installedModules;

                //if (ModuleDescriptors?.Any() ?? false)
                //    _installedModules = ModuleDescriptors
                //        .Select(pd => pd.moduleDescriptor as ModuleDescriptorBaseInfo).ToList();

                return _installedModules;
            }
            set => _installedModules = value;
        }

        /// <summary>
        /// Gets or sets the list of module names which will be uninstalled
        /// </summary>
        public virtual IList<string> ModuleNamesToUninstall { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of module names which will be deleted
        /// </summary>
        public virtual IList<string> ModuleNamesToDelete { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of module names which will be installed
        /// </summary>
        public virtual IList<(string SystemName, Guid? UserGuid)> ModuleNamesToInstall { get; set; } =
            [];


        /// <summary>
        /// Gets or sets the list of module which are not compatible with the current version
        /// </summary>
        /// <remarks>
        /// Key - the system name of module.
        /// Value - the reason of incompatibility.
        /// </remarks>
        [JsonIgnore]
        public virtual IDictionary<string, ModuleIncompatibleType> IncompatibleModules { get; set; }

        /// <summary>
        /// Gets or sets the list of assembly loaded collisions
        /// </summary>
        [JsonIgnore]
        public virtual IList<ModuleLoadedAssemblyInfo> AssemblyLoadedCollision { get; set; }

        /// <summary>
        /// Gets or sets a collection of module descriptors of all deployed modules
        /// </summary>
        [JsonIgnore]
        public virtual IList<(ModuleDescriptor moduleDescriptor, bool needToDeploy)> ModuleDescriptors { get; set; }

        #endregion
    }
}