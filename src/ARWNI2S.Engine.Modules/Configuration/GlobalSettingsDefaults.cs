using ARWNI2S.Caching;
using ARWNI2S.Core.Data.Configuration;

namespace ARWNI2S.Core.Configuration
{
    /// <summary>
    /// Represents default values related to settings
    /// </summary>
    public static partial class GlobalSettingsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new("ni2s.setting.all.dictionary.", EntityCacheDefaults<Setting>.Prefix);

        #endregion
    }
}