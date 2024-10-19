using ARWNI2S.Infrastructure;
using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Services.Plugins;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Data.Configuration;
using ARWNI2S.Node.Runtime.Security;
using ARWNI2S.Node.Services.Common;
using ARWNI2S.Node.Services.Installation;
using ARWNI2S.Node.Services.Plugins;
using ARWNI2S.Node.Services.Security;
using System.Globalization;

namespace ARWNI2S.Node.Runtime.Services.Installation
{
    internal class ConsoleDatabaseInstaller : IDatabaseInstaller
    {
        private readonly NI2SSettings _nI2SSettings;
        private readonly Lazy<IInstallationService> _installationService;
        private readonly IEngineFileProvider _fileProvider;
        private readonly Lazy<IPermissionService> _permissionService;
        private readonly Lazy<IModuleService> _moduleService;
        private readonly Lazy<IStaticCacheManager> _staticCacheManager;
        private readonly Lazy<INodeHelper> _nodeHelper;

        public ConsoleDatabaseInstaller(NI2SSettings nI2SSettings,
            Lazy<IInstallationService> installationService,
            IEngineFileProvider fileProvider,
            Lazy<IPermissionService> permissionService,
            Lazy<IModuleService> moduleService,
            Lazy<IStaticCacheManager> staticCacheManager,
            Lazy<INodeHelper> nodeHelper)
        {
            _nI2SSettings = nI2SSettings;
            _installationService = installationService;
            _fileProvider = fileProvider;
            _permissionService = permissionService;
            _moduleService = moduleService;
            _staticCacheManager = staticCacheManager;
            _nodeHelper = nodeHelper;
        }

        public async Task InstallDatabaseAsync()
        {
            if (DataSettingsManager.IsDatabaseInstalled())
                return;

            //validate permissions
            var dirsToCheck = _fileProvider.GetDirectoriesWrite();
            foreach (var dir in dirsToCheck)
                if (!_fileProvider.CheckPermissions(dir, false, true, true, false))
                {
                    // ModelState.AddModelError(string.Empty, string.Format(_locService.Value.GetResource("ConfigureDirectoryPermissions"), CurrentOSUser.FullName, dir));
                    Console.WriteLine("TODO: PERMISSION ERROR");
                    Environment.Exit(0);
                }

            var filesToCheck = _fileProvider.GetFilesWrite();
            foreach (var file in filesToCheck)
            {
                if (!_fileProvider.FileExists(file))
                    continue;

                if (!_fileProvider.CheckPermissions(file, false, true, true, true))
                {
                    // ModelState.AddModelError(string.Empty, string.Format(_locService.Value.GetResource("ConfigureFilePermissions"), CurrentOSUser.FullName, file));
                    Console.WriteLine("TODO: PERMISSION ERROR");
                    Environment.Exit(0);
                }
            }

            var dataSettings = _nI2SSettings.Get<DataConfig>();

            Console.WriteLine("No database configured:");
            Console.WriteLine("Install (y/n)");

            if (Console.ReadKey(true).Key != ConsoleKey.Y)
            {
                Console.WriteLine("Can't continue.");
                Environment.Exit(0);
            }

            try
            {
                var dataProviderType = await PromptForDataProviderType();
                var dataProvider = DataProviderManager.GetDataProvider(dataProviderType);

                string connectionString = await PromptForConnectionString();
                if (!connectionString.Replace(" ", "").Contains(dataProviderType == DataProviderType.SqlServer ? ";InitialCatalog=" : ";Database=")) 
                {
                    string databaseName = await PromptForDatabaseName();
                    connectionString += $"{(dataProviderType == DataProviderType.SqlServer ? "; Initial Catalog = " : "; Database = ")}{databaseName}";
                }
                if (!connectionString.EndsWith(';'))
                    connectionString += ";";

                if (string.IsNullOrEmpty(connectionString))
                    throw new NodeException("TODO: LOCALIZATION - ConnectionStringWrongFormat");

                DataSettingsManager.SaveSettings(new DataConfig
                {
                    DataProvider = dataProviderType,
                    ConnectionString = connectionString
                }, _fileProvider);

                //check whether database exists
                if (!await dataProvider.DatabaseExistsAsync())
                {
                    var collation = await PromptForCollationAsync();

                    dataProvider.CreateDatabase(collation);
                }

                dataProvider.InitializeDatabase();

                var cultureInfo = new CultureInfo(CommonServicesDefaults.DefaultLanguageCulture);
                var regionInfo = new RegionInfo(CommonServicesDefaults.DefaultLanguageCulture);

                var languagePackInfo = (DownloadUrl: string.Empty, Progress: 0);
                if (_nI2SSettings.Get<InstallationConfig>().InstallRegionalResources)
                {
                    ////try to get CultureInfo and RegionInfo
                    //if (model.Country != null)
                    //    try
                    //    {
                    //        cultureInfo = new CultureInfo(model.Country[3..]);
                    //        regionInfo = new RegionInfo(model.Country[3..]);
                    //    }
                    //    catch
                    //    {
                    //        // ignored
                    //    }

                    //get URL to download language pack
                    //if (cultureInfo.Name != CommonServicesDefaults.DefaultLanguageCulture)
                    //{
                    //    try
                    //    {
                    //        var languageCode = _locService.Value.GetCurrentLanguage().Code[0..2];
                    //        var resultString = await _draCoHttpClient.Value.InstallationCompletedAsync(model.AdminEmail, languageCode, cultureInfo.Name);
                    //        var result = JsonConvert.DeserializeAnonymousType(resultString,
                    //            new { Message = string.Empty, LanguagePack = new { Culture = string.Empty, Progress = 0, DownloadLink = string.Empty } });

                    //        if (result != null && result.LanguagePack.Progress > CommonServicesDefaults.LanguagePackMinTranslationProgressToInstall)
                    //        {
                    //            languagePackInfo.DownloadUrl = result.LanguagePack.DownloadLink;
                    //            languagePackInfo.Progress = result.LanguagePack.Progress;
                    //        }
                    //    }
                    //    catch
                    //    {
                    //        // ignored
                    //    }
                    //}

                    //upload CLDR
                    //await _uploadService.Value.UploadLocalePatternAsync(cultureInfo);
                }

                //now resolve installation service
                (string admin, string password) = await PromptForUserAndPasswordAsync();
                await _installationService.Value.InstallRequiredDataAsync(admin, password, languagePackInfo, regionInfo, cultureInfo);

                //prepare modules to install
                _moduleService.Value.ClearInstalledModulesList();

                var modulesIgnoredDuringInstallation = new List<string>();
                if (!string.IsNullOrEmpty(_nI2SSettings.Get<InstallationConfig>().DisabledModules))
                {
                    modulesIgnoredDuringInstallation = _nI2SSettings.Get<InstallationConfig>().DisabledModules
                        .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(moduleName => moduleName.Trim()).ToList();
                }

                var modules = (await _moduleService.Value.GetModuleDescriptorsAsync<IModule>(LoadModulesMode.All))
                    .Where(moduleDescriptor => !modulesIgnoredDuringInstallation.Contains(moduleDescriptor.SystemName))
                    .OrderBy(moduleDescriptor => moduleDescriptor.Group).ThenBy(moduleDescriptor => moduleDescriptor.DisplayOrder)
                    .ToList();

                if (modulesIgnoredDuringInstallation.Count == 1 && modulesIgnoredDuringInstallation[0] == "*")
                    modules.Clear();

                foreach (var module in modules)
                {
                    await _moduleService.Value.PrepareModuleToInstallAsync(module.SystemName, checkDependencies: false);
                }

                //register default permissions
                var permissionProviders = new List<Type> { typeof(StandardPermissionProvider) };
                foreach (var providerType in permissionProviders)
                {
                    var provider = (IPermissionProvider)Activator.CreateInstance(providerType);
                    await _permissionService.Value.InstallPermissionsAsync(provider);
                }

                if(await PromptRestartOrContinue())
                {
                    if (DataSettingsManager.IsDatabaseInstalled())
                        return;

                    //restart application
                    _nodeHelper.Value.RestartAppDomain();
                }

            }
            catch (Exception exception)
            {
                await _staticCacheManager.Value.ClearAsync();

                //clear provider settings if something got wrong
                DataSettingsManager.SaveSettings(new DataConfig(), _fileProvider);

                Console.WriteLine(exception.ToString());
            }

        }

        private static async Task<DataProviderType> PromptForDataProviderType()
        {
            Console.WriteLine("Select a data provider:");
            Console.WriteLine("\t1 - SqlServer (default)");
            Console.WriteLine("\t2 - MySql");
            Console.WriteLine("\t3 - PostgreSQL");
            Console.WriteLine("\t4 - Other");

            var key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter && !char.IsDigit(key.KeyChar) && (key.KeyChar < '1' || key.KeyChar > '4'))
                key = Console.ReadKey(true);

            if (key.KeyChar == '1' || key.Key == ConsoleKey.Enter)
                return await Task.FromResult(DataProviderType.SqlServer);
            else if (key.KeyChar == '2')
                return await Task.FromResult(DataProviderType.MySql);
            else if (key.KeyChar == '3')
                return await Task.FromResult(DataProviderType.PostgreSQL);

            return await Task.FromResult(DataProviderType.Unknown);
        }

        private static async Task<string> PromptForConnectionString()
        {
            Console.WriteLine("Enter connection string:");
            return await Task.FromResult(Console.ReadLine());
        }

        private static async Task<string> PromptForDatabaseName()
        {
            Console.WriteLine("Enter database name:");
            return await Task.FromResult(Console.ReadLine());
        }

        private static async Task<string> PromptForCollationAsync()
        {
            Console.WriteLine("Database not exists. Create with collation: (enter for default)");
            return await Task.FromResult(Console.ReadLine());
        }

        private static async Task<(string, string)> PromptForUserAndPasswordAsync()
        {
            Console.WriteLine("Enter admin username:");
            var admin = Console.ReadLine();
            Console.WriteLine("Enter admin password:");
            var password = Console.ReadLine();
            return await Task.FromResult((admin, password));
        }

        private static async Task<bool> PromptRestartOrContinue()
        {
            Console.WriteLine("NI2S node installed. Maybe restart needed.");
            Console.WriteLine("Restart (y/n)");

            var key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Y && key.Key != ConsoleKey.N)
            {
                key = Console.ReadKey(true);
            }

            return await Task.FromResult(key.Key == ConsoleKey.Y);
        }

        //public virtual async Task<IActionResult> Index(InstallModel model)
        //{
        //    if (DataSettingsManager.IsDatabaseInstalled())
        //        return RedirectToRoute("Homepage");

        //    model.InstallRegionalResources = _nI2SSettings.Get<InstallationConfig>().InstallRegionalResources;

        //    PrepareAvailableDataProviders(model);
        //    PrepareLanguageList(model);
        //    PrepareCountryList(model);

        //    //Consider granting access rights to the resource to the ASP.NET request identity. 
        //    //ASP.NET has a base process identity 
        //    //(typically {MACHINE}\ASPNET on IIS 5 or Network Service on IIS 6 and IIS 7, 
        //    //and the configured application pool identity on IIS 7.5) that is used if the application is not impersonating.
        //    //If the application is impersonating via <identity impersonate="true"/>, 
        //    //the identity will be the anonymous user (typically IUSR_MACHINENAME) or the authenticated request user.

        //    //validate permissions
        //    var dirsToCheck = _fileProvider.GetDirectoriesWrite();
        //    foreach (var dir in dirsToCheck)
        //        if (!_fileProvider.CheckPermissions(dir, false, true, true, false))
        //            ModelState.AddModelError(string.Empty, string.Format(_locService.Value.GetResource("ConfigureDirectoryPermissions"), CurrentOSUser.FullName, dir));

        //    var filesToCheck = _fileProvider.GetFilesWrite();
        //    foreach (var file in filesToCheck)
        //    {
        //        if (!_fileProvider.FileExists(file))
        //            continue;

        //        if (!_fileProvider.CheckPermissions(file, false, true, true, true))
        //            ModelState.AddModelError(string.Empty, string.Format(_locService.Value.GetResource("ConfigureFilePermissions"), CurrentOSUser.FullName, file));
        //    }

        //    if (!ModelState.IsValid)
        //        return View(model);

        //    try
        //    {
        //        var dataProvider = DataProviderManager.GetDataProvider(model.DataProvider);

        //        var connectionString = model.ConnectionStringRaw ? model.ConnectionString : dataProvider.BuildConnectionString(model);

        //        if (string.IsNullOrEmpty(connectionString))
        //            throw new MetalinkException(_locService.Value.GetResource("ConnectionStringWrongFormat"));

        //        DataSettingsManager.SaveSettings(new DataConfig
        //        {
        //            DataProvider = model.DataProvider,
        //            ConnectionString = connectionString
        //        }, _fileProvider);

        //        if (model.CreateDatabaseIfNotExists)
        //        {
        //            try
        //            {
        //                dataProvider.CreateDatabase(model.Collation);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new MetalinkException(string.Format(_locService.Value.GetResource("DatabaseCreationError"), ex.Message));
        //            }
        //        }
        //        else
        //        {
        //            //check whether database exists
        //            if (!await dataProvider.DatabaseExistsAsync())
        //                throw new MetalinkException(_locService.Value.GetResource("DatabaseNotExists"));
        //        }

        //        dataProvider.InitializeDatabase();

        //        var cultureInfo = new CultureInfo(CommonServicesDefaults.DefaultLanguageCulture);
        //        var regionInfo = new RegionInfo(CommonServicesDefaults.DefaultLanguageCulture);

        //        var languagePackInfo = (DownloadUrl: string.Empty, Progress: 0);
        //        if (model.InstallRegionalResources)
        //        {
        //            //try to get CultureInfo and RegionInfo
        //            if (model.Country != null)
        //                try
        //                {
        //                    cultureInfo = new CultureInfo(model.Country[3..]);
        //                    regionInfo = new RegionInfo(model.Country[3..]);
        //                }
        //                catch
        //                {
        //                    // ignored
        //                }

        //            //get URL to download language pack
        //            //if (cultureInfo.Name != CommonServicesDefaults.DefaultLanguageCulture)
        //            //{
        //            //    try
        //            //    {
        //            //        var languageCode = _locService.Value.GetCurrentLanguage().Code[0..2];
        //            //        var resultString = await _draCoHttpClient.Value.InstallationCompletedAsync(model.AdminEmail, languageCode, cultureInfo.Name);
        //            //        var result = JsonConvert.DeserializeAnonymousType(resultString,
        //            //            new { Message = string.Empty, LanguagePack = new { Culture = string.Empty, Progress = 0, DownloadLink = string.Empty } });

        //            //        if (result != null && result.LanguagePack.Progress > CommonServicesDefaults.LanguagePackMinTranslationProgressToInstall)
        //            //        {
        //            //            languagePackInfo.DownloadUrl = result.LanguagePack.DownloadLink;
        //            //            languagePackInfo.Progress = result.LanguagePack.Progress;
        //            //        }
        //            //    }
        //            //    catch
        //            //    {
        //            //        // ignored
        //            //    }
        //            //}

        //            //upload CLDR
        //            await _uploadService.Value.UploadLocalePatternAsync(cultureInfo);
        //        }

        //        //now resolve installation service
        //        await _installationService.Value.InstallRequiredDataAsync(model.AdminEmail, model.AdminPassword, languagePackInfo, regionInfo, cultureInfo);

        //        //prepare modules to install
        //        _moduleService.Value.ClearInstalledModulesList();

        //        var modulesIgnoredDuringInstallation = new List<string>();
        //        if (!string.IsNullOrEmpty(_nI2SSettings.Get<InstallationConfig>().DisabledModules))
        //        {
        //            modulesIgnoredDuringInstallation = _nI2SSettings.Get<InstallationConfig>().DisabledModules
        //                .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(moduleName => moduleName.Trim()).ToList();
        //        }

        //        var modules = (await _moduleService.Value.GetModuleDescriptorsAsync<IModule>(LoadModulesMode.All))
        //            .Where(moduleDescriptor => !modulesIgnoredDuringInstallation.Contains(moduleDescriptor.SystemName))
        //            .OrderBy(moduleDescriptor => moduleDescriptor.Group).ThenBy(moduleDescriptor => moduleDescriptor.DisplayOrder)
        //            .ToList();

        //        if (modulesIgnoredDuringInstallation.Count == 1 && modulesIgnoredDuringInstallation[0] == "*")
        //            modules.Clear();

        //        foreach (var module in modules)
        //        {
        //            await _moduleService.Value.PrepareModuleToInstallAsync(module.SystemName, checkDependencies: false);
        //        }

        //        //register default permissions
        //        var permissionProviders = new List<Type> { typeof(StandardPermissionProvider) };
        //        foreach (var providerType in permissionProviders)
        //        {
        //            var provider = (IPermissionProvider)Activator.CreateInstance(providerType);
        //            await _permissionService.Value.InstallPermissionsAsync(provider);
        //        }

        //        return View(new InstallModel { RestartUrl = Url.RouteUrl("Homepage") });

        //    }
        //    catch (Exception exception)
        //    {
        //        await _staticCacheManager.Value.ClearAsync();

        //        //clear provider settings if something got wrong
        //        DataSettingsManager.SaveSettings(new DataConfig(), _fileProvider);

        //        ModelState.AddModelError(string.Empty, string.Format(_locService.Value.GetResource("SetupFailed"), exception.Message));
        //    }

        //    return View(model);
        //}
    }
}
