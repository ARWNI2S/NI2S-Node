using ARWNI2S.Engine.Caching;
using ARWNI2S.Engine.Configuration.Data;

namespace ARWNI2S.Engine.Configuration
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