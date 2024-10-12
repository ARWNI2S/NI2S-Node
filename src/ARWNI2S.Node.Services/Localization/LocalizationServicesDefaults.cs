using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Data.Entities.Localization;

namespace ARWNI2S.Node.Services.Localization
{
    /// <summary>
    /// Represents default values related to localization services
    /// </summary>
    public static partial class LocalizationServicesDefaults
    {
        #region Locales

        /// <summary>
        /// Gets a prefix of locale resources for the admin area
        /// </summary>
        public static string AdminLocaleStringResourcesPrefix => "Admin.";

        /// <summary>
        /// Gets a prefix of locale resources for enumerations 
        /// </summary>
        public static string EnumLocaleStringResourcesPrefix => "Enums.";

        /// <summary>
        /// Gets a prefix of locale resources for permissions 
        /// </summary>
        public static string PermissionLocaleStringResourcesPrefix => "Permission.";

        /// <summary>
        /// Gets a prefix of locale resources for module friendly names 
        /// </summary>
        public static string ModuleNameLocaleStringResourcesPrefix => "Modules.FriendlyName.";

        #endregion

        #region Caching defaults

        #region Languages

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : node ID
        /// {1} : show hidden records?
        /// </remarks>
        public static CacheKey LanguagesAllCacheKey => new("ni2s.language.all.{0}-{1}", LanguagesByNodePrefix, EntityCacheDefaults<Language>.AllPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : node ID
        /// </remarks>
        public static string LanguagesByNodePrefix => "ni2s.language.all.{0}";

        #endregion

        #region Locales

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static CacheKey LocaleStringResourcesAllPublicCacheKey => new("ni2s.localestringresource.bylanguage.public.{0}", EntityCacheDefaults<LocaleStringResource>.Prefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static CacheKey LocaleStringResourcesAllAdminCacheKey => new("ni2s.localestringresource.bylanguage.admin.{0}", EntityCacheDefaults<LocaleStringResource>.Prefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static CacheKey LocaleStringResourcesAllCacheKey => new("ni2s.localestringresource.bylanguage.{0}", EntityCacheDefaults<LocaleStringResource>.Prefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : resource key
        /// </remarks>
        public static CacheKey LocaleStringResourcesByNameCacheKey => new("ni2s.localestringresource.byname.{0}-{1}", LocaleStringResourcesByNamePrefix, EntityCacheDefaults<LocaleStringResource>.Prefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static string LocaleStringResourcesByNamePrefix => "ni2s.localestringresource.byname.{0}";

        #endregion

        #region Localized properties

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : entity ID
        /// {2} : locale key group
        /// {3} : locale key
        /// </remarks>
        public static CacheKey LocalizedPropertyCacheKey => new("ni2s.localizedproperty.value.{0}-{1}-{2}-{3}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : locale key group
        /// {2} : locale key
        /// </remarks>
        public static CacheKey LocalizedPropertiesCacheKey => new("ni2s.localizedproperty.all.{0}-{1}-{2}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public static CacheKey LocalizedPropertyLookupCacheKey => new("ni2s.localizedproperty.value.{0}");

        #endregion

        #endregion
    }
}
