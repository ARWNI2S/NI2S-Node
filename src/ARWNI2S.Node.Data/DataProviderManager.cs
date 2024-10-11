using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Data.Configuration;
using ARWNI2S.Node.Data.DataProviders;

namespace ARWNI2S.Node.Data
{
    /// <summary>
    /// Represents the data provider manager
    /// </summary>
    public partial class DataProviderManager : IDataProviderManager
    {
        #region Methods

        /// <summary>
        /// Gets data provider by specific type
        /// </summary>
        /// <param name="dataProviderType">Data provider type</param>
        /// <returns></returns>
        public static IServerDataProvider GetDataProvider(DataProviderType dataProviderType)
        {
            return dataProviderType switch
            {
                DataProviderType.SqlServer => new MsSqlServerDataProvider(),
                DataProviderType.MySql => new MySqlServerDataProvider(),
                DataProviderType.PostgreSQL => new PostgreSqlServerDataProvider(),
                _ => throw new ServerException($"Not supported data provider name: '{dataProviderType}'"),
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        public IServerDataProvider DataProvider
        {
            get
            {
                var dataProviderType = Singleton<DataConfig>.Instance.DataProvider;

                return GetDataProvider(dataProviderType);
            }
        }

        #endregion
    }
}
