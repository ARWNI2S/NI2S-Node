// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Configuration;
using ARWNI2S.Engine.Configuration;
using Newtonsoft.Json;

namespace ARWNI2S.Engine.Simulation
{
    internal class SimulationConfig : IConfig
    {
        [JsonIgnore]
        public string Name => NI2SConfigurationDefaults.GetConfigName<SimulationConfig>();

        public bool IsDevelopment { get; set; } = true;
        public bool IsDebug { get; set; } = true;
    }
}
