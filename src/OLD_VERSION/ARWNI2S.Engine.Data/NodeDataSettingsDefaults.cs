namespace ARWNI2S.Data
{
    /// <summary>
    /// Represents default values related to data settings
    /// </summary>
    public static partial class NodeDataSettingsDefaults
    {
        /// <summary>
        /// Gets a path to the file that was used in old Server versions to contain data settings
        /// </summary>
        public static string ObsoleteFilePath => "~/Node_Data/Settings.txt";

        /// <summary>
        /// Gets a path to the file that contains data settings
        /// </summary>
        public static string FilePath => "~/Node_Data/dataSettings.json";
    }
}