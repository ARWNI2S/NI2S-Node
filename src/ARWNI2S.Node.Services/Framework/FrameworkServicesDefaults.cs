using ARWNI2S.Node.Caching;
using ARWNI2S.Node.Data.Entities.Framework;

namespace ARWNI2S.Node.Services.Framework
{
    /// <summary>
    /// Represents default values related to directory services
    /// </summary>
    public static partial class FrameworkServicesDefaults
    {
        #region Caching defaults

        #region Currencies

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey CurrenciesAllCacheKey => new("ni2s.currency.all.{0}", EntityCacheDefaults<Currency>.AllPrefix);

        #endregion

        #endregion
    }
}
