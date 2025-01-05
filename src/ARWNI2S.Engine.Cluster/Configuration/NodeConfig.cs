using ARWNI2S.Configuration;

namespace ARWNI2S.Cluster.Configuration
{
    public class NodeConfig : IConfig
    {
        public string NodeName { get; set; }
        public string NodeId { get; set; }
        public NodeType Type { get; set; } = NodeType.NonEngine;
    }
}
