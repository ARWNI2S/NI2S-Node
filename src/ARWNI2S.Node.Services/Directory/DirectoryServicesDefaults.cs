using ARWNI2S.Node.Core.Caching;
using ARWNI2S.Node.Core.Entities.Directory;

namespace ARWNI2S.Node.Services.Directory
{
    /// <summary>
    /// Represents default values related to directory services
    /// </summary>
    public static partial class DirectoryServicesDefaults
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
