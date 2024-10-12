namespace ARWNI2S.Node.Data
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
        INI2SDataProvider DataProvider { get; }

        #endregion
    }
}
