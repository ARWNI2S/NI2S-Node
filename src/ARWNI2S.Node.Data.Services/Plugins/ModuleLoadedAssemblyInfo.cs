namespace ARWNI2S.Node.Data.Services.Plugins
{
    /// <summary>
    /// Represents an information about assembly which loaded by modules
    /// </summary>
    public partial class ModuleLoadedAssemblyInfo
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="shortName">Assembly short name</param>
        /// <param name="assemblyInMemory">Assembly version</param>
        public ModuleLoadedAssemblyInfo(string shortName, Version assemblyInMemory)
        {
            ShortName = shortName;
            References = [];
            AssemblyInMemory = assemblyInMemory;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Assembly in memory
        /// </summary>
        public Version AssemblyInMemory { get; }

        /// <summary>
        /// Gets the short assembly name
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Gets a list of all mentioned module-assembly pairs
        /// </summary>
        public List<(string ModuleName, Version AssemblyVersion)> References { get; }

        /// <summary>
        /// Gets a list of modules that conflict with the loaded assembly version
        /// </summary>
        public IList<(string ModuleName, Version AssemblyVersion)> Collisions =>
            References.Where(reference => !reference.AssemblyVersion.Equals(AssemblyInMemory)).ToList();

        #endregion
    }
}
