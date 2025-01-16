using ARWNI2S.Caching;

namespace ARWNI2S.Cluster.Nodes
{
    internal class NodeServicesDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : node GUID
        /// </remarks>
        public static CacheKey NodeByGuidCacheKey => new("NI2S.node.byguid.{0}");
    }
}