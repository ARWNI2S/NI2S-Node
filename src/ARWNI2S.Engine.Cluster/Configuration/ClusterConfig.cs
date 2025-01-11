using ARWNI2S.Configuration;
using ARWNI2S.Engine.Configuration;
using Newtonsoft.Json;
using Orleans.Configuration;

namespace ARWNI2S.Cluster.Configuration
{
    public enum StorageType
    {
        Local = 0,
        AzureStorage = 1,
        AdoNet = 2,
        DynamoDB = 3,
        ServiceFabric = 4,
        Consul = 5,
        ZooKeeper = 6,
    }

    public class ClusterConfig : IConfig
    {
        [JsonIgnore]
        public string Name => NI2SConfigurationDefaults.GetConfigName<ClusterConfig>();

        public string ClusterId { get; set; } = ClusterOptions.DefaultClusterId;

        public string ServiceId { get; set; } = ClusterOptions.DefaultServiceId;

        public StorageType Storage { get; set; }

        public bool LanOnly { get; set; } = true;
        public bool SameSite { get; set; } = true;
        public bool UseSsl { get; set; } = false;
        public string SslCertPath { get; set; } = string.Empty;
    }
}
