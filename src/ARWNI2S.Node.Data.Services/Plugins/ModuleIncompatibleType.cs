namespace ARWNI2S.Node.Data.Services.Plugins
{
    public enum ModuleIncompatibleType
    {
        /// <summary>
        /// The module isn't compatible with the current version
        /// </summary>
        NotCompatibleWithCurrentVersion,
        /// <summary>
        /// The main assembly isn't found
        /// </summary>
        MainAssemblyNotFound,
        /// <summary>
        /// Another version of the same library is already loaded in memory
        /// </summary>
        HasCollisions
    }
}
