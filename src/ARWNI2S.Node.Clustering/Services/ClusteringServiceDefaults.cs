using ARWNI2S.Caching;

namespace ARWNI2S.Clustering.Services
{
    /// <summary>
    /// Represents default values related to nodes services
    /// </summary>
    public static partial class ClusteringServiceDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey NodeMappingIdsCacheKey => new("ni2s.nodemapping.ids.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey NodeMappingsCacheKey => new("ni2s.nodemapping.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity name
        /// </remarks>
        public static CacheKey NodeMappingExistsCacheKey => new("ni2s.nodemapping.exists.{0}");

        #endregion
    }
}