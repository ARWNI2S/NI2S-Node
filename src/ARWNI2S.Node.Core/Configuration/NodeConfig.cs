using ARWNI2S.Infrastructure;
using ARWNI2S.Infrastructure.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ARWNI2S.Node.Core.Configuration
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
        public string NodeName { get; set; } = $"{Environment.MachineName}:{Environment.ProcessId}";

        public Guid NodeId { get; set; } = new (CommonHelper.GenerateRandomGuidString());

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
