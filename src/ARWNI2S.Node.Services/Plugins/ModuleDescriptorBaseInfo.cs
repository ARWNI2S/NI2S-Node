using Newtonsoft.Json;

namespace ARWNI2S.Node.Services.Plugins
{
    /// <summary>
    /// Represents base info of module descriptor
    /// </summary>
    public partial class ModuleDescriptorBaseInfo : IComparable<ModuleDescriptorBaseInfo>
    {
        /// <summary>
        /// Gets or sets the module system name
        /// </summary>
        [JsonProperty(PropertyName = "SystemName")]
        public virtual string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        [JsonProperty(PropertyName = "Version")]
        public virtual string Version { get; set; }

        /// <summary>
        /// Compares this instance with a specified ModuleDescriptorBaseInfo object
        /// </summary>
        /// <param name="other">The ModuleDescriptorBaseInfo to compare with this instance</param>
        /// <returns>An integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified parameter</returns>
        public int CompareTo(ModuleDescriptorBaseInfo other)
        {
            return string.Compare(SystemName, other.SystemName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether this instance and another specified ModuleDescriptor object have the same SystemName
        /// </summary>
        /// <param name="value">The ModuleDescriptor to compare to this instance</param>
        /// <returns>True if the SystemName of the value parameter is the same as the SystemName of this instance; otherwise, false</returns>
        public override bool Equals(object value)
        {
            return SystemName?.Equals((value as ModuleDescriptorBaseInfo)?.SystemName) ?? false;
        }

        /// <summary>
        /// Returns the hash code for this module descriptor
        /// </summary>
        /// <returns>A 32-bit signed integer hash code</returns>
        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }

        /// <summary>
        /// Gets a copy of base info of module descriptor
        /// </summary>
        [JsonIgnore]
        public virtual ModuleDescriptorBaseInfo GetBaseInfoCopy =>
            new()
            { SystemName = SystemName, Version = Version };
    }
}