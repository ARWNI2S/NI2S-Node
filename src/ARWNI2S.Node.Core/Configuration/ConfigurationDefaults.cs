﻿namespace ARWNI2S.Node.Core.Configuration
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class ConfigurationDefaults
    {
        /// <summary>
        /// Gets the path to file that contains node settings
        /// </summary>
        public static string NodeSettingsFilePath => "Node_Data/nodesettings.json";

        /// <summary>
        /// Gets the path to file that contains node settings for specific hosting environment
        /// </summary>
        /// <remarks>0 - Environment name</remarks>
        public static string NodeSettingsEnvironmentFilePath => "Node_Data/nodesettings.{0}.json";
    }
}
