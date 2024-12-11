namespace ARWNI2S.Configuration
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class NI2SConfigurationDefaults
    {
        /// <summary>
        /// Gets the path to file that contains node settings
        /// </summary>
        public static string NI2SSettingsFilePath => "Node_Data/nodesettings.json";

        /// <summary>
        /// Gets the path to file that contains node settings for specific hosting environment
        /// </summary>
        /// <remarks>0 - Environment name</remarks>
        public static string NI2SSettingsEnvironmentFilePath => "Node_Data/nodesettings.{0}.json";
    }
}
