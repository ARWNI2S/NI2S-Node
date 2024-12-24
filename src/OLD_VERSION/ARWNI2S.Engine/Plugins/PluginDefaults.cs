using ARWNI2S.Engine.Caching;

namespace ARWNI2S.Engine.Plugins
{
    /// <summary>
    /// Represents default values related to plugins
    /// </summary>
    public static partial class PluginDefaults
    {

        /// <summary>
        /// Gets the path to file that contains installed plugin system names
        /// </summary>
        public static string PluginsInfoFilePath => "~/Node_Data/plugins.json";

        /// <summary>
        /// Gets the path to plugins folder
        /// </summary>
        public static string Path => "~/Plugins";

        /// <summary>
        /// Gets the path to plugins folder
        /// </summary>
        public static string UploadedPath => "~/Plugins/Uploaded";

        /// <summary>
        /// Gets the plugins folder name
        /// </summary>
        public static string PathName => "Plugins";

        /// <summary>
        /// Gets the path to plugins refs folder
        /// </summary>
        public static string RefsPathName => "refs";

        /// <summary>
        /// Gets the name of the plugin description file
        /// </summary>
        public static string DescriptionFileName => "plugin.json";

        /// <summary>
        /// Gets the plugins logo filename
        /// </summary>
        public static string LogoFileName => "logo";

        /// <summary>
        /// Gets supported extensions of logo file
        /// </summary>
        public static List<string> SupportedLogoImageExtensions => ["jpg", "png", "gif"];

        /// <summary>
        /// Gets the path to temp directory with uploads
        /// </summary>
        public static string UploadsTempPath => "~/Node_Data/TempUploads";

        /// <summary>
        /// Gets the name of the file containing information about the uploaded items
        /// </summary>
        public static string UploadedItemsFileName => "uploadedItems.json";

        /// <summary>
        /// Gets a key for caching plugins for admin navigation
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// </remarks>
        public static CacheKey AdminNavigationPluginsCacheKey => new("ni2s.plugins.adminnavigation.{0}", AdminNavigationPluginsPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AdminNavigationPluginsPrefix => "ni2s.plugins.adminnavigation.";
    }
}