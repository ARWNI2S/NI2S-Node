// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NI2S.Node.Core.Configuration
{
    /// <summary>
    /// Represents the node settings
    /// </summary>
    public partial class NodeSettings
    {
        #region Fields

        private readonly Dictionary<Type, IConfig> _configurations = new();

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a instance of <see cref="NodeSettings"/>.
        /// </summary>
        /// <param name="configurations">A <see cref="IList{IConfig}"/> containing <see cref="IConfig"/> configurations.</param>
        public NodeSettings(IList<IConfig> configurations = null)
        {
            _configurations = configurations
                ?.OrderBy(config => config.GetOrder())
                ?.ToDictionary(config => config.GetType(), config => config)
                ?? new Dictionary<Type, IConfig>();
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
                throw new NI2SException($"No configuration with type '{typeof(TConfig)}' found");

            return config;
        }

        /// <summary>
        /// Update app settings
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
