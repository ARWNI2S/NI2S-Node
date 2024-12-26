using ARWNI2S.Cluster;
using ARWNI2S.Extensibility;

namespace ARWNI2S.Engine.Cluster
{
    public interface IClusterNode : INiisNode
    {
        IModuleCollection Modules { get; }
    }
}
