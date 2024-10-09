namespace ARWNI2S.Node.Data.Services.Plugins
{
    /// <summary>
    /// Represents the module updated event
    /// </summary>
    public partial class ModuleUpdatedEvent
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="module">Updated module</param>
        public ModuleUpdatedEvent(ModuleDescriptor module)
        {
            Module = module;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Updated module
        /// </summary>
        public ModuleDescriptor Module { get; }

        #endregion
    }
}