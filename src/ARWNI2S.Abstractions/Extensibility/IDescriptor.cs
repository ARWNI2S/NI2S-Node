namespace ARWNI2S.Extensibility
{
    /// <summary>
    /// Represents descriptor of the engine extension (plugin or module)
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