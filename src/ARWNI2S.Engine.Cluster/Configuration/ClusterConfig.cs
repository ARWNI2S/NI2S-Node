// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Configuration;
using ARWNI2S.Engine.Configuration;
using Newtonsoft.Json;
using Orleans.Configuration;

namespace ARWNI2S.Cluster.Configuration
{
    public class ClusterConfig : IConfig
    {
        [JsonIgnore]
        public string Name => NI2SConfigurationDefaults.GetConfigName<ClusterConfig>();

        public string ClusterId { get; set; } = ClusterOptions.DefaultClusterId;

        public string ServiceId { get; set; } = ClusterOptions.DefaultServiceId;

        public bool LanOnly { get; set; } = true;
        public bool SameSite { get; set; } = true;
        public bool UseSsl { get; set; } = false;
        public string SslCertPath { get; set; } = string.Empty;
    }
}
