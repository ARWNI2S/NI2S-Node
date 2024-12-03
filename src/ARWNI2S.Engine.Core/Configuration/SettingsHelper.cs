using ARWNI2S.Engine;
using ARWNI2S.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ARWNI2S.Configuration
{
    /// <summary>
    /// Represents the NI2S settings helper
    /// </summary>
    public partial class SettingsHelper
    {
        #region Fields

        private static Dictionary<string, int> _configurationOrder;

        #endregion

        #region Methods

        /// <summary>
        /// Create ni2s settings with the passed configurations and save it to the file
        /// </summary>
        /// <param name="configurations">Configurations to save</param>
        /// <param name="fileProvider">File provider</param>
        /// <param name="overwrite">Whether to overwrite ni2ssettings file</param>
        /// <returns>Node settings</returns>
        public static NI2SSettings SaveSettings(IList<IConfig> configurations, INiisFileProvider fileProvider, bool overwrite = true)
        {
            ArgumentNullException.ThrowIfNull(configurations);

            _configurationOrder ??= configurations.ToDictionary(config => config.Name, config => config.GetOrder());

            //create NI2S settings
            var niisSettings = Singleton<NI2SSettings>.Instance ?? new NI2SSettings();
            niisSettings.Update(configurations);
            Singleton<NI2SSettings>.Instance = niisSettings;

            //create file if not exists
            var filePath = fileProvider.MapPath(ConfigurationDefaults.SettingsFilePath);
            var fileExists = fileProvider.FileExists(filePath);
            fileProvider.CreateFile(filePath);

            //get raw configuration parameters
            var configuration = JsonConvert.DeserializeObject<NI2SSettings>(fileProvider.ReadAllText(filePath, Encoding.UTF8))
                ?.Configuration
                ?? [];
            foreach (var config in configurations)
            {
                configuration[config.Name] = JToken.FromObject(config);
            }

            //sort configurations for display by order (e.g. data configuration with 0 will be the first)
            niisSettings.Configuration = configuration
                .SelectMany(outConfig => _configurationOrder.Where(inConfig => inConfig.Key == outConfig.Key).DefaultIfEmpty(),
                    (outConfig, inConfig) => new { OutConfig = outConfig, InConfig = inConfig })
                .OrderBy(config => config.InConfig.Value)
                .Select(config => config.OutConfig)
                .ToDictionary(config => config.Key, config => config.Value);

            //save ni2s settings to the file
            if (!fileExists || overwrite)
            {
                var text = JsonConvert.SerializeObject(niisSettings, Formatting.Indented);//, new JsonSerializerSettings { ContractResolver = new SecretContractResolver() });
                fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
            }

            return niisSettings;
        }

        #endregion
    }
}