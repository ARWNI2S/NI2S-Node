﻿using ARWNI2S.Node.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ARWNI2S.Node.Configuration
{
    /// <summary>
    /// Represents the NI2S settings
    /// </summary>
    public partial class NiisSettings
    {
        #region Fields

        private readonly Dictionary<Type, IConfig> _configurations = [];

        #endregion

        #region Ctor

        public NiisSettings(IList<IConfig> configurations = null)
        {
            _configurations = configurations
                ?.OrderBy(config => config.GetOrder())
                ?.ToDictionary(config => config.GetType(), config => config)
                ?? [];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets raw configuration parameters
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> Configuration { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get configuration parameters by type
        /// </summary>
        /// <typeparam name="TConfig">Configuration type</typeparam>
        /// <returns>Configuration parameters</returns>
        public TConfig Get<TConfig>() where TConfig : class, IConfig
        {
            if (_configurations[typeof(TConfig)] is not TConfig config)
                throw new NiisException($"No configuration with type '{typeof(TConfig)}' found");

            return config;
        }

        /// <summary>
        /// Update node settings
        /// </summary>
        /// <param name="configurations">Configurations to update</param>
        public void Update(IList<IConfig> configurations)
        {
            foreach (var config in configurations)
            {
                _configurations[config.GetType()] = config;
            }
        }

        #endregion
    }
}