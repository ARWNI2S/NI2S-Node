namespace ARWNI2S.Node.Data
{
    /// <summary>
    /// Represents default values related to data settings
    /// </summary>
    public static partial class ServerDataSettingsDefaults
    {
        /// <summary>
        /// Gets a path to the file that was used in old Server versions to contain data settings
        /// </summary>
        public static string ObsoleteFilePath => "~/App_Data/Settings.txt";

        /// <summary>
        /// Gets a path to the file that contains data settings
        /// </summary>
        public static string FilePath => "~/App_Data/dataSettings.json";
    }
}