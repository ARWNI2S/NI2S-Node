﻿namespace ARWNI2S.Node.Configuration
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class ConfigurationDefaults
    {
        /// <summary>
        /// Gets the path to file that contains node settings
        /// </summary>
        public static string SettingsFilePath => "Node_Data/appsettings.json";

        /// <summary>
        /// Gets the path to file that contains node settings for specific hosting environment
        /// </summary>
        /// <remarks>0 - Environment name</remarks>
        public static string SettingsEnvironmentFilePath => "Node_Data/appsettings.{0}.json";
    }
}
