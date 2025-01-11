using ARWNI2S.Cluster.Builder;

namespace ARWNI2S.Cluster.Extensibility
{
    internal static class ClusterNodeModuleExtensions
    {
        internal static IClusterNodeBuilder UseNodeModules(this IClusterNodeBuilder builder)
        {

            return builder;
        }
    }
}
