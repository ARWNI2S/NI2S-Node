// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Configuration
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class ConfigDefaults
    {
        /// <summary>
        /// Gets the path to file that contains node settings
        /// </summary>
        public static string SettingsFilePath => "Node_Data/nodesettings.json";

        /// <summary>
        /// Gets the path to file that contains node settings for specific hosting environment
        /// </summary>
        /// <remarks>0 - Environment name</remarks>
        public static string SettingsEnvironmentFilePath => "Node_Data/nodesettings.{0}.json";
    }
}
