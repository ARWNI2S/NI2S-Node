﻿using ARWNI2S.Node.Core;
using ARWNI2S.Node.Data.Configuration;
using ARWNI2S.Node.Data.DataProviders;
using ARWNI2S.Node.Infrastructure;

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
        public static INiisDataProvider GetDataProvider(DataProviderType dataProviderType)
        {
            return dataProviderType switch
            {
                DataProviderType.SqlServer => new MsSqlServerDataProvider(),
                DataProviderType.MySql => new MySqlServerDataProvider(),
                DataProviderType.PostgreSQL => new PostgreSqlServerDataProvider(),
                _ => throw new NiisException($"Not supported data provider name: '{dataProviderType}'"),
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        public INiisDataProvider DataProvider
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
