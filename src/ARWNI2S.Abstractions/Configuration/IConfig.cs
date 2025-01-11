// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Newtonsoft.Json;

namespace ARWNI2S.Configuration
{
    /// <summary>
    /// Represents a configuration from app settings
    /// </summary>
    public partial interface IConfig
    {
        /// <summary>
        /// Gets a section name to load configuration
        /// </summary>
        [JsonIgnore]
        string Name => GetType().Name;

        /// <summary>
        /// Gets an order of configuration
        /// </summary>
        /// <returns>Order</returns>
        public int GetOrder() => 1;
    }
}