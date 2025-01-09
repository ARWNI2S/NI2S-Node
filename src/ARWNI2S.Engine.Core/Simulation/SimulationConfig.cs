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
