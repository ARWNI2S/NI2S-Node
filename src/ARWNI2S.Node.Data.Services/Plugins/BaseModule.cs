namespace ARWNI2S.Node.Data.Services.Plugins
{
    /// <summary>
    /// Base module
    /// </summary>
    public abstract partial class BaseModule : IModule
    {
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public virtual string GetConfigurationPageUrl()
        {
            return null;
        }

        /// <summary>
        /// Gets or sets the module descriptor
        /// </summary>
        public virtual ModuleDescriptor ModuleDescriptor { get; set; }

        /// <summary>
        /// Install module
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual Task InstallAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Uninstall module
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual Task UninstallAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Update module
        /// </summary>
        /// <param name="currentVersion">Current version of module</param>
        /// <param name="targetVersion">New version of module</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual Task UpdateAsync(string currentVersion, string targetVersion)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Prepare module to the uninstallation
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual Task PrepareModuleToUninstallAsync()
        {
            //any can put any custom validation logic here
            //throw an exception if this module cannot be uninstalled
            //for example, requires some other certain modules to be uninstalled first
            return Task.CompletedTask;
        }
    }
}
