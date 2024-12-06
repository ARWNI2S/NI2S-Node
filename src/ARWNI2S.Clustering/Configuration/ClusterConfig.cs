using ARWNI2S.Configuration;
using ARWNI2S.Security.Secrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ARWNI2S.Clustering.Configuration
{
    /// <summary>
    /// Represents simulation clustering type enumeration
    /// </summary>
    public enum SimulationClusteringType
    {
        [EnumMember(Value = "localhost")]
        Localhost,
        [EnumMember(Value = "azure")]
        AzureStorage,
        [EnumMember(Value = "sqlserver")]
        SqlServer,
        [EnumMember(Value = "redis")]
        Redis
    }

    /// <summary>
    /// Represents cluster configuration parameters
    /// </summary>
    public partial class ClusterConfig : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating if is a development cluster.
        /// </summary>
        public bool IsDevelopment { get; private set; } = true;

        /// <summary>
        /// Gets or sets the cluster identifier.
        /// </summary>
        public string ClusterId { get; set; } = "default";

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        public string ServiceId { get; set; } = "ni2s";

        /// <summary>
        /// Gets or sets the silo storage clustering type.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public SimulationClusteringType SiloStorageClustering { get; private set; } = SimulationClusteringType.Localhost;

        /// <summary>
        /// Gets or sets connection string for clustering storage
        /// </summary>
        [Secret]
        public string ConnectionString { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets addresses of known nodes to connect with
        /// </summary>
        public string KnownNodes { get; private set; } = string.Empty;
    }
}
