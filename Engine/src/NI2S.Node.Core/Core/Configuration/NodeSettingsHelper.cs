// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NI2S.Node.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NI2S.Node.Core.Configuration
{
    /// <summary>
    /// Represents the node settings helper
    /// </summary>
    public partial class NodeSettingsHelper
    {
        #region Fields

        private static Dictionary<string, int> _configurationOrder;

        #endregion

        #region Methods

        /// <summary>
        /// Create node settings with the passed configurations and save it to the file.
        /// </summary>
        /// <param name="configurations">Configurations to save.</param>
        /// <param name="fileProvider">File provider.</param>
        /// <param name="overwrite">Whether to overwrite nodesettings file (default is <see langword="true"/>).</param>
        /// <returns>Node settings.</returns>
        /* 045 */
        public static NodeSettings SaveNodeSettings(IList<IConfig> configurations, INodeFileProvider fileProvider, bool overwrite = true)
        {
            if (configurations is null)
                throw new ArgumentNullException(nameof(configurations));

            _configurationOrder ??= configurations.ToDictionary(config => config.Name, config => config.GetOrder());

            //create node settings
            var nodeSettings = Singleton<NodeSettings>.Instance ?? new NodeSettings();
            nodeSettings.Update(configurations);
            Singleton<NodeSettings>.Instance = nodeSettings;

            //create file if not exists
            var filePath = fileProvider.MapPath(ConfigurationDefaults.NodeSettingsFilePath);
            var fileExists = fileProvider.FileExists(filePath);
            fileProvider.CreateFile(filePath);

            //get raw configuration parameters
            var configuration = JsonConvert.DeserializeObject<NodeSettings>(fileProvider.ReadAllText(filePath, Encoding.UTF8))
                ?.Configuration
                ?? new();
            foreach (var config in configurations)
            {
                configuration[config.Name] = JToken.FromObject(config);
            }

            //sort configurations for display by order (e.g. data configuration with 0 will be the first)
            nodeSettings.Configuration = configuration
                .SelectMany(outConfig => _configurationOrder.Where(inConfig => inConfig.Key == outConfig.Key).DefaultIfEmpty(),
                    (outConfig, inConfig) => new { OutConfig = outConfig, InConfig = inConfig })
                .OrderBy(config => config.InConfig.Value)
                .Select(config => config.OutConfig)
                .ToDictionary(config => config.Key, config => config.Value);

            //save node settings to the file
            if (!fileExists || overwrite)
            {
                var text = JsonConvert.SerializeObject(nodeSettings, Formatting.Indented);
                fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
            }

            return nodeSettings;
        }

        #endregion
    }
}
