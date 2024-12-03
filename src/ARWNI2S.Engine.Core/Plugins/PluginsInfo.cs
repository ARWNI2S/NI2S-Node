using ARWNI2S.Extensibility;
using ARWNI2S.Infrastructure;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;


namespace ARWNI2S.Plugins
{
    /// <summary>
    /// Represents an information about plugins
    /// </summary>
    public partial class PluginsInfo : IPluginsInfo
    {
        #region Fields

        private IList<PluginDescriptorBaseInfo> _installedPlugins = [];

        protected readonly INiisFileProvider _fileProvider;

        #endregion

        #region Utilities

        /// <summary>
        /// Deserialize PluginInfo from json
        /// </summary>
        /// <param name="json">Json data of PluginInfo</param>
        /// <returns>True if data are loaded, otherwise False</returns>
        protected virtual void DeserializePluginInfo(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            var pluginsInfo = JsonConvert.DeserializeObject<PluginsInfo>(json);

            if (pluginsInfo == null)
                return;

            InstalledPlugins = pluginsInfo.InstalledPlugins;
            PluginNamesToUninstall = pluginsInfo.PluginNamesToUninstall;
            PluginNamesToDelete = pluginsInfo.PluginNamesToDelete;
            PluginNamesToInstall = pluginsInfo.PluginNamesToInstall;
        }

        /// <summary>
        /// Check whether the directory is a plugin directory
        /// </summary>
        /// <param name="directoryName">Directory name</param>
        /// <returns>Result of check</returns>
        protected bool IsPluginDirectory(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName))
                return false;

            //get parent directory
            var parent = _fileProvider.GetParentDirectory(directoryName);
            if (string.IsNullOrEmpty(parent))
                return false;

            //directory is directly in plugins directory
            if (!_fileProvider.GetDirectoryNameOnly(parent).Equals(PluginDefaults.PathName, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// Get list of description files-plugin descriptors pairs
        /// </summary>
        /// <param name="directoryName">Plugin directory name</param>
        /// <returns>Original and parsed description files</returns>
        protected IList<(string DescriptionFile, PluginDescriptor PluginDescriptor)> GetDescriptionFilesAndDescriptors(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName))
                throw new ArgumentNullException(nameof(directoryName));

            var result = new List<(string DescriptionFile, PluginDescriptor PluginDescriptor)>();

            //try to find description files in the plugin directory
            var files = _fileProvider.GetFiles(directoryName, PluginDefaults.DescriptionFileName, false);

            //populate result list
            foreach (var descriptionFile in files)
            {
                //skip files that are not in the plugin directory
                if (!IsPluginDirectory(_fileProvider.GetDirectoryName(descriptionFile)))
                    continue;

                //load plugin descriptor from the file
                var text = _fileProvider.ReadAllText(descriptionFile, Encoding.UTF8);
                var pluginDescriptor = PluginDescriptor.GetPluginDescriptorFromText(text);

                result.Add((descriptionFile, pluginDescriptor));
            }

            //sort list by display order. NOTE: Lowest DisplayOrder will be first i.e 0 , 1, 1, 1, 5, 10
            //it's required: https://www.dragoncorp.org/boards/topic/17455/load-plugins-based-on-their-displayorder-on-startup
            result = result.OrderBy(item => item.PluginDescriptor.DisplayOrder).ToList();

            return result;
        }

        #endregion

        #region Ctor

        public PluginsInfo(INiisFileProvider fileProvider)
        {
            _fileProvider = fileProvider ?? CommonHelper.DefaultFileProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get plugins info
        /// </summary>
        /// <returns>
        /// The true if data are loaded, otherwise False
        /// </returns>
        public virtual void LoadPluginInfo()
        {
            //check whether plugins info file exists
            var filePath = _fileProvider.MapPath(PluginDefaults.PluginsInfoFilePath);

            //try to get plugin info from the JSON file
            var text = _fileProvider.FileExists(filePath)
                ? _fileProvider.ReadAllText(filePath, Encoding.UTF8)
                : string.Empty;

            DeserializePluginInfo(text);

            var pluginDescriptors = new List<(PluginDescriptor pluginDescriptor, bool needToDeploy)>();
            var incompatiblePlugins = new Dictionary<string, PluginIncompatibleType>();

            //ensure plugins directory is created
            var pluginsDirectory = _fileProvider.MapPath(PluginDefaults.Path);
            _fileProvider.CreateDirectory(pluginsDirectory);

            //load plugin descriptors from the plugin directory
            foreach (var item in GetDescriptionFilesAndDescriptors(pluginsDirectory))
            {
                var descriptionFile = item.DescriptionFile;
                var pluginDescriptor = item.PluginDescriptor;

                //skip descriptor of plugin that is going to be deleted
                if (PluginNamesToDelete.Contains(pluginDescriptor.SystemName))
                    continue;

                //ensure that plugin is compatible with the current version
                if (!pluginDescriptor.SupportedVersions.Contains(Version.CURRENT_VERSION, StringComparer.InvariantCultureIgnoreCase))
                {
                    incompatiblePlugins.Add(pluginDescriptor.SystemName, PluginIncompatibleType.NotCompatibleWithCurrentVersion);
                    continue;
                }

                //some more validation
                if (string.IsNullOrEmpty(pluginDescriptor.SystemName?.Trim()))
                    throw new NiisException($"A plugin '{descriptionFile}' has no system name. Try assigning the plugin a unique name and recompiling.");

                if (pluginDescriptors.Any(p => p.pluginDescriptor.Equals(pluginDescriptor)))
                    throw new NiisException($"A plugin with '{pluginDescriptor.SystemName}' system name is already defined");

                //set 'Installed' property
                pluginDescriptor.Installed = InstalledPlugins.Select(pd => pd.SystemName)
                    .Any(pluginName => pluginName.Equals(pluginDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase));

                try
                {
                    //try to get plugin directory
                    var pluginDirectory = _fileProvider.GetDirectoryName(descriptionFile);
                    if (string.IsNullOrEmpty(pluginDirectory))
                        throw new NiisException($"Directory cannot be resolved for '{_fileProvider.GetFileName(descriptionFile)}' description file");

                    //get list of all library files in the plugin directory (not in the bin one)
                    pluginDescriptor.PluginFiles = _fileProvider.GetFiles(pluginDirectory, "*.dll", false)
                        .Where(file => IsPluginDirectory(_fileProvider.GetDirectoryName(file)))
                        .ToList();

                    //try to find a main plugin assembly file
                    var mainPluginFile = pluginDescriptor.PluginFiles.FirstOrDefault(file =>
                    {
                        var fileName = _fileProvider.GetFileName(file);
                        return fileName.Equals(pluginDescriptor.AssemblyFileName, StringComparison.InvariantCultureIgnoreCase);
                    });

                    //file with the specified name not found
                    if (mainPluginFile == null)
                    {
                        //so plugin is incompatible
                        incompatiblePlugins.Add(pluginDescriptor.SystemName, PluginIncompatibleType.MainAssemblyNotFound);
                        continue;
                    }

                    var pluginName = pluginDescriptor.SystemName;

                    //if it's found, set it as original assembly file
                    pluginDescriptor.OriginalAssemblyFile = mainPluginFile;

                    //need to deploy if plugin is already installed
                    var needToDeploy = InstalledPlugins.Select(pd => pd.SystemName).Contains(pluginName);

                    //also, deploy if the plugin is only going to be installed now
                    needToDeploy = needToDeploy || PluginNamesToInstall.Any(pluginInfo => pluginInfo.SystemName.Equals(pluginName));

                    //finally, exclude from deploying the plugin that is going to be deleted
                    needToDeploy = needToDeploy && !PluginNamesToDelete.Contains(pluginName);

                    //mark plugin as successfully deployed
                    pluginDescriptors.Add((pluginDescriptor, needToDeploy));
                }
                catch (ReflectionTypeLoadException exception)
                {
                    //get all loader exceptions
                    var error = exception.LoaderExceptions.Aggregate($"Plugin '{pluginDescriptor.FriendlyName}'. ",
                        (message, nextMessage) => $"{message}{nextMessage?.Message ?? string.Empty}{Environment.NewLine}");

                    throw new NiisException(error, exception);
                }
                catch (Exception exception)
                {
                    //add a plugin name, this way we can easily identify a problematic plugin
                    throw new NiisException($"Plugin '{pluginDescriptor.FriendlyName}'. {exception.Message}", exception);
                }
            }

            IncompatiblePlugins = incompatiblePlugins;
            PluginDescriptors = pluginDescriptors;
        }


        /// <summary>
        /// Save plugins info to the file
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SaveAsync()
        {
            //save the file
            var filePath = _fileProvider.MapPath(PluginDefaults.PluginsInfoFilePath);
            var text = JsonConvert.SerializeObject(this, Formatting.Indented);
            await _fileProvider.WriteAllTextAsync(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// Save plugins info to the file
        /// </summary>
        public virtual void Save()
        {
            //save the file
            var filePath = _fileProvider.MapPath(PluginDefaults.PluginsInfoFilePath);
            var text = JsonConvert.SerializeObject(this, Formatting.Indented);
            _fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// Create copy from another instance of IPluginsInfo interface
        /// </summary>
        /// <param name="pluginsInfo">Plugins info</param>
        public virtual void CopyFrom(IPluginsInfo pluginsInfo)
        {
            InstalledPlugins = pluginsInfo.InstalledPlugins?.ToList() ?? [];
            PluginNamesToUninstall = pluginsInfo.PluginNamesToUninstall?.ToList() ?? [];
            PluginNamesToDelete = pluginsInfo.PluginNamesToDelete?.ToList() ?? [];
            PluginNamesToInstall = pluginsInfo.PluginNamesToInstall?.ToList() ?? [];
            AssemblyLoadedCollision = pluginsInfo.AssemblyLoadedCollision?.ToList();
            PluginDescriptors = pluginsInfo.PluginDescriptors;
            IncompatiblePlugins = pluginsInfo.IncompatiblePlugins?.ToDictionary(item => item.Key, item => item.Value);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of all installed plugin
        /// </summary>
        public virtual IList<PluginDescriptorBaseInfo> InstalledPlugins
        {
            get
            {
                if (_installedPlugins?.Any() ?? false)
                    return _installedPlugins;

                //if (PluginDescriptors?.Any() ?? false)
                //    _installedPlugins = PluginDescriptors
                //        .Select(pd => pd.pluginDescriptor as PluginDescriptorBaseInfo).ToList();

                return _installedPlugins;
            }
            set => _installedPlugins = value;
        }

        /// <summary>
        /// Gets or sets the list of plugin names which will be uninstalled
        /// </summary>
        public virtual IList<string> PluginNamesToUninstall { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of plugin names which will be deleted
        /// </summary>
        public virtual IList<string> PluginNamesToDelete { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of plugin names which will be installed
        /// </summary>
        public virtual IList<(string SystemName, Guid? UserGuid)> PluginNamesToInstall { get; set; } =
            [];


        /// <summary>
        /// Gets or sets the list of plugin which are not compatible with the current version
        /// </summary>
        /// <remarks>
        /// Key - the system name of plugin.
        /// Value - the reason of incompatibility.
        /// </remarks>
        [JsonIgnore]
        public virtual IDictionary<string, PluginIncompatibleType> IncompatiblePlugins { get; set; }

        /// <summary>
        /// Gets or sets the list of assembly loaded collisions
        /// </summary>
        [JsonIgnore]
        public virtual IList<PluginLoadedAssemblyInfo> AssemblyLoadedCollision { get; set; }

        /// <summary>
        /// Gets or sets a collection of plugin descriptors of all deployed plugins
        /// </summary>
        [JsonIgnore]
        public virtual IList<(PluginDescriptor pluginDescriptor, bool needToDeploy)> PluginDescriptors { get; set; }

        #endregion
    }
}