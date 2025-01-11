// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Newtonsoft.Json;

namespace ARWNI2S.Extensibility
{
    /// <summary>
    /// Represents base info of plugin descriptor
    /// </summary>
    public abstract class DescriptorBase : IComparable<DescriptorBase>
    {
        /// <summary>
        /// Gets or sets the plugin system name
        /// </summary>
        [JsonProperty(PropertyName = "SystemName")]
        public virtual string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        [JsonProperty(PropertyName = "Version")]
        public virtual string Version { get; set; }

        /// <summary>
        /// Compares this instance with a specified DescriptorBase object
        /// </summary>
        /// <param name="other">The DescriptorBase to compare with this instance</param>
        /// <returns>An integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified parameter</returns>
        public int CompareTo(DescriptorBase other)
        {
            return string.Compare(SystemName, other.SystemName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether this instance and another specified PluginDescriptor object have the same SystemName
        /// </summary>
        /// <param name="value">The PluginDescriptor to compare to this instance</param>
        /// <returns>True if the SystemName of the value parameter is the same as the SystemName of this instance; otherwise, false</returns>
        public override bool Equals(object value)
        {
            return SystemName?.Equals((value as DescriptorBase)?.SystemName) ?? false;
        }

        /// <summary>
        /// Returns the hash code for this plugin descriptor
        /// </summary>
        /// <returns>A 32-bit signed integer hash code</returns>
        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }

        /// <summary>
        /// Gets a copy of base info of plugin descriptor
        /// </summary>
        [JsonIgnore]
        public virtual DescriptorBase GetDescriptorCopy
        {
            get
            {
                var copy = (DescriptorBase)Activator.CreateInstance(GetType());
                copy.SystemName = SystemName;
                copy.Version = Version;
                return copy;
            }
        }
    }
}