namespace ARWNI2S.Node.Data.Services.Plugins
{
    /// <summary>
    /// Interface denoting plug-in attributes that are displayed throughout 
    /// the editing interface.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        string GetConfigurationPageUrl();

        /// <summary>
        /// Gets or sets the module descriptor
        /// </summary>
        ModuleDescriptor ModuleDescriptor { get; set; }

        /// <summary>
        /// Install module
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InstallAsync();

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UninstallAsync();

        /// <summary>
        /// Update module
        /// </summary>
        /// <param name="currentVersion">Current version of module</param>
        /// <param name="targetVersion">New version of module</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateAsync(string currentVersion, string targetVersion);

        /// <summary>
        /// Prepare module to the uninstallation
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task PrepareModuleToUninstallAsync();
    }
}
