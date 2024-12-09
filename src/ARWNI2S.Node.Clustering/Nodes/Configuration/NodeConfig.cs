using ARWNI2S.Configuration;
using ARWNI2S.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Runtime.Serialization;

namespace ARWNI2S.Clustering.Nodes.Configuration
{
    /// <summary>
    /// Represents simulation clustering type enumeration
    /// </summary>
    public enum NodeType
    {
        [EnumMember(Value = "frontline")]
        Frontline,
        [EnumMember(Value = "narrator")]
        Narrator,
        [EnumMember(Value = "other")]
        Other
    }

    public partial class NodeConfig : IConfig
    {
        public string NodeName { get; set; } = $"{Dns.GetHostName()}:{Environment.ProcessId}";

        public Guid NodeId { get; set; } = new(CommonHelper.GenerateRandomGuidString());

        public ushort Port { get; set; } = 46600;

        [JsonIgnore]
        public ushort RelayPort { get; } = 46633;

        /// <summary>
        /// Gets or sets the node type.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public NodeType NodeType { get; private set; } = NodeType.Other;

        /// <summary>
        /// Gets or sets a value indicating whether to enable editor node accessor.
        /// </summary>
        public bool EnableEditor { get; private set; }

        /// <summary>
        /// Gets or sets a value to auto scale cluster.
        /// </summary>
        public bool AutoScale { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if is a development cluster.
        /// </summary>
        public bool IsDevelopment { get; private set; } = true;

        public int GetOrder() => 0;

    }
}
