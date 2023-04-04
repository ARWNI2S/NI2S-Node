// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Newtonsoft.Json;

namespace NI2S.Node.Core.Configuration
{
    /// <summary>
    /// Represents a configuration section from node settings
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
