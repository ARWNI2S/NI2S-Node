using ARWNI2S.Engine.Core;

namespace ARWNI2S.Cluster
{
    public interface INiisNode : INiisEntity
    {
        Guid NodeId { get; }

        NodeType NodeType { get; }
    }
}
