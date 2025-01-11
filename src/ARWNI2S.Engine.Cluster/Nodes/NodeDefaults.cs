using ARWNI2S.Caching;

namespace ARWNI2S.Cluster.Nodes
{
    /// <summary>
    /// Represents default values related to nodes services
    /// </summary>
    public static partial class NodeDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey NodeMappingIdsCacheKey => new("NI2S.nodemapping.ids.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey NodeMappingsCacheKey => new("NI2S.nodemapping.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity name
        /// </remarks>
        public static CacheKey NodeMappingExistsCacheKey => new("NI2S.nodemapping.exists.{0}");

        #endregion
    }
}