namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Represents descriptor of the application extension (module or theme)
    /// </summary>
    public partial interface IDescriptor
    {
        /// <summary>
        /// Gets or sets the system name
        /// </summary>
        string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the friendly name
        /// </summary>
        string FriendlyName { get; set; }
    }
}