namespace ARWNI2S.Node.Services.Installation
{
    /// <summary>
    /// Represents default values related to installation services
    /// </summary>
    public static partial class InstallationDefaults
    {
        /// <summary>
        /// Gets a path to the localization resources file
        /// </summary>
        public static string LocalizationResourcesPath => "~/Node_Data/Localization/";

        /// <summary>
        /// Gets a localization resources file extension
        /// </summary>
        public static string LocalizationResourcesFileExtension => "ni2sloc.xml";
    }
}