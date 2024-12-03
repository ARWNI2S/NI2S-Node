using ARWNI2S.Configuration;

namespace ARWNI2S.Services.Localization
{
    /// <summary>
    /// Localization settings
    /// </summary>
    public partial class LocalizationSettings : ISettings
    {
        /// <summary>
        /// Default admin area language identifier
        /// </summary>
        public int DefaultAdminLanguageId { get; set; }

        /// <summary>
        /// Use images for language selection
        /// </summary>
        public bool UseImagesForLanguageSelection { get; set; }

        /// <summary>
        /// A value indicating whether SEO friendly URLs with multiple languages are enabled
        /// </summary>
        public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }

        /// <summary>
        /// A value indicating whether we should detect the current language by a user region (browser settings)
        /// </summary>
        public bool AutomaticallyDetectLanguage { get; set; }

        /// <summary>
        /// A value indicating whether to load all LocaleStringResource records on engine startup
        /// </summary>
        public bool LoadAllLocaleRecordsOnStartup { get; set; }

        /// <summary>
        /// A value indicating whether to load all LocalizedProperty records on engine startup
        /// </summary>
        public bool LoadAllLocalizedPropertiesOnStartup { get; set; }

        /// <summary>
        /// A value indicating whether to load all search engine friendly names (slugs) on engine startup
        /// </summary>
        public bool LoadAllVirtualFileRecordsOnStartup { get; set; }

        /// <summary>
        /// A value indicating whether to we should ignore RTL language property for admin area.
        /// It's useful for administrators with RTL languages for public node but preferring LTR for admin area
        /// </summary>
        public bool IgnoreRtlPropertyForAdminArea { get; set; }
    }
}