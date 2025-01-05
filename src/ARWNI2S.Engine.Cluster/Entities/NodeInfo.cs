using ARWNI2S.Engine.Data;

namespace ARWNI2S.Cluster.Entities
{
    public class NodeInfo : DataEntity, INiisNode
    {
        public Guid NodeId { get; set; }

        public NodeType NodeType { get; set; }
    }
}
