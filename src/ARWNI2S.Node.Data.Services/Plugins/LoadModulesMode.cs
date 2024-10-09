namespace ARWNI2S.Node.Data.Services.Plugins
{
    /// <summary>
    /// Represents a mode to load modules
    /// </summary>
    public enum LoadModulesMode
    {
        /// <summary>
        /// All (Installed and Not installed)
        /// </summary>
        All = 0,

        /// <summary>
        /// Installed only
        /// </summary>
        InstalledOnly = 10,

        /// <summary>
        /// Not installed only
        /// </summary>
        NotInstalledOnly = 20
    }
}