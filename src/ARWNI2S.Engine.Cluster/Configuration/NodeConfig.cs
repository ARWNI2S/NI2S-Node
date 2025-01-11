// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Configuration;
using ARWNI2S.Engine.Configuration;
using Newtonsoft.Json;

namespace ARWNI2S.Cluster.Configuration
{
    public class NodeConfig : IConfig
    {
        [JsonIgnore]
        public string Name => NI2SConfigurationDefaults.GetConfigName<NodeConfig>();

        /// <summary>
        /// 
        /// </summary>
        public string HostName { get; set; } = System.Environment.MachineName;

        /// <summary>
        /// 
        /// </summary>
        public Guid NodeId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        public NodeType Type { get; set; } = NodeType.NonEngine;

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ports { get; set; }

        /// <summary>
        /// Gets an order of configuration
        /// </summary>
        /// <returns>Order</returns>
        public int GetOrder() => -1; //display first one
    }
}
