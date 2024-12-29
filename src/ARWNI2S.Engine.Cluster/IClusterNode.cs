using ARWNI2S.Extensibility;

namespace ARWNI2S.Cluster
{
    public interface IClusterNode : INiisNode
    {
        IModuleCollection Modules { get; }

    }
}
