﻿using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Data.Entities.Configuration;

namespace ARWNI2S.Node.Data.Services.Configuration
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