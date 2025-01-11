// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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
