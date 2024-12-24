using ARWNI2S.Engine.Core;
using ARWNI2S.Extensibility;

namespace ARWNI2S.Cluster
{
    internal interface INiisNode : INiisEntity
    {
        IModuleCollection Modules { get; }
    }
}
