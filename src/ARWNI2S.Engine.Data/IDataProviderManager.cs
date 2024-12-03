namespace ARWNI2S.Data
{
    /// <summary>
    /// Represents a data provider manager
    /// </summary>
    public partial interface IDataProviderManager
    {
        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        INiisDataProvider DataProvider { get; }

        #endregion
    }
}
