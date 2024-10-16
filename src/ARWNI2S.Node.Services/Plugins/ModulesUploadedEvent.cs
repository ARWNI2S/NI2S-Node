namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Modules uploaded event
    /// </summary>
    public partial class ModulesUploadedEvent
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="uploadedModules">Uploaded modules</param>
        public ModulesUploadedEvent(IList<ModuleDescriptor> uploadedModules)
        {
            UploadedModules = uploadedModules;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Uploaded modules
        /// </summary>
        public IList<ModuleDescriptor> UploadedModules { get; }

        #endregion
    }
}