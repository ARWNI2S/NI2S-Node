using ARWNI2S.Configuration;
using Newtonsoft.Json;

namespace ARWNI2S.Engine.Configuration
{
    /// <summary>
    /// Represents plugin configuration parameters
    /// </summary>
    public partial class PluginConfig : IConfig
    {
        /// <summary>
        /// Gets a section name to load configuration
        /// </summary>
        [JsonIgnore]
        public string Name => NI2SConfigurationDefaults.PluginConfigName;

        /// <summary>
        /// Gets or sets a value indicating whether to load an assembly into the load-from context, bypassing some security checks.
        /// </summary>
        public bool UseUnsafeLoadAssembly { get; set; } = true;

        /// <inheritdoc/>
        public int GetOrder() => 3;
    }
}
