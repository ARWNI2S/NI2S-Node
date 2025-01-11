using ARWNI2S.Configuration;
using ARWNI2S.Engine.Configuration;
using Newtonsoft.Json;

namespace ARWNI2S.Engine.Caching
{
    /// <summary>
    /// Represents cache configuration parameters
    /// </summary>
    public partial class CacheConfig : IConfig
    {
        [JsonIgnore]
        public string Name => NI2SConfigurationDefaults.GetConfigName<CacheConfig>();

        /// <summary>
        /// Gets or sets the default cache time in minutes
        /// </summary>
        public int DefaultCacheTime { get; protected set; } = 60;

        /// <summary>
        /// Gets or sets the short term cache time in minutes
        /// </summary>
        public int ShortTermCacheTime { get; protected set; } = 3;

        /// <summary>
        /// Gets or sets whether to disable linq2db query cache
        /// </summary>
        public bool LinqDisableQueryCache { get; protected set; } = false;

        /// <inheritdoc/>
        public int GetOrder() => 2;
    }
}