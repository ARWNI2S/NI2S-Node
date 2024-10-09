using ARWNI2S.Node.Core.Caching;

namespace ARWNI2S.Node.Data.Services.Plugins
{
    /// <summary>
    /// Represents default values related to modules
    /// </summary>
    public static partial class ModuleServicesDefaults
    {

        /// <summary>
        /// Gets the path to file that contains installed module system names
        /// </summary>
        public static string ModulesInfoFilePath => "~/App_Data/modules.json";

        /// <summary>
        /// Gets the path to modules folder
        /// </summary>
        public static string Path => "~/Modules";

        /// <summary>
        /// Gets the path to modules folder
        /// </summary>
        public static string UploadedPath => "~/Modules/Uploaded";

        /// <summary>
        /// Gets the modules folder name
        /// </summary>
        public static string PathName => "Modules";

        /// <summary>
        /// Gets the path to modules refs folder
        /// </summary>
        public static string RefsPathName => "refs";

        /// <summary>
        /// Gets the name of the module description file
        /// </summary>
        public static string DescriptionFileName => "module.json";

        /// <summary>
        /// Gets the modules logo filename
        /// </summary>
        public static string LogoFileName => "logo";

        /// <summary>
        /// Gets supported extensions of logo file
        /// </summary>
        public static List<string> SupportedLogoImageExtensions => ["jpg", "png", "gif"];

        /// <summary>
        /// Gets the path to temp directory with uploads
        /// </summary>
        public static string UploadsTempPath => "~/App_Data/TempUploads";

        /// <summary>
        /// Gets the name of the file containing information about the uploaded items
        /// </summary>
        public static string UploadedItemsFileName => "uploadedItems.json";

        /// <summary>
        /// Gets a key for caching modules for admin navigation
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// </remarks>
        public static CacheKey AdminNavigationModulesCacheKey => new("ni2s.modules.adminnavigation.{0}", AdminNavigationModulesPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AdminNavigationModulesPrefix => "ni2s.modules.adminnavigation.";
    }
}