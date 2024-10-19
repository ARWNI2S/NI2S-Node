using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Core.Caching
{
    /// <summary>
    /// Represents cache configuration parameters
    /// </summary>
    public partial class CacheConfig : IConfig
    {
        /// <summary>
        /// Gets or sets the default cache time in minutes
        /// </summary>
        public int DefaultCacheTime { get; private set; } = 60;

        /// <summary>
        /// Gets or sets the short term cache time in minutes
        /// </summary>
        public int ShortTermCacheTime { get; private set; } = 3;

        /// <summary>
        /// Gets or sets the bundled files cache time in minutes
        /// </summary>
        public int BundledFilesCacheTime { get; private set; } = 120;

        /// <inheritdoc/>
        public int GetOrder() => 2;
    }
}